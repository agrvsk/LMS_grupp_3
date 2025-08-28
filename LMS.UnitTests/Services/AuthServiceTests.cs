using Domain.Models.Entities;
using Domain.Models.Exceptions;
using LMS.Services;
using LMS.Shared.DTOs.AuthDtos;
using LMS.UnitTests.Setups;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LMS.UnitTests.Services
{
    public class AuthServiceTests : ServiceTestBase
    {
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _authService = new AuthService(
                MockMapper.Object,
                MockUserManager.Object,
                MockRoleManager.Object,
                JwtOptions
                );
        }

        #region [ValidateUserAsync]
        [Fact]
        [Trait("AuthService", "Validate User")]
        public async Task ValidateUserAsync_ValidUser_ReturnsTrue()
        {
            var userDto = new UserAuthDto { UserName = "testuser", Password = "password" };
            var appUser = CreateTestUser(userDto.UserName);

            MockUserManager.Setup(m => m.FindByNameAsync(userDto.UserName)).ReturnsAsync(appUser);
            MockUserManager.Setup(m => m.CheckPasswordAsync(appUser, userDto.Password)).ReturnsAsync(true);

            var result = await _authService.ValidateUserAsync(userDto);

            Assert.True(result);
        }

        [Fact]
        [Trait("AuthService", "Validate User")]
        public async Task ValidateUserAsync_InvalidUser_ReturnsFalse()
        {
            var userDto = new UserAuthDto { UserName = "unknown", Password = "password" };
            MockUserManager.Setup(m => m.FindByNameAsync(userDto.UserName)).ReturnsAsync((ApplicationUser)null!);

            var result = await _authService.ValidateUserAsync(userDto);

            Assert.False(result);
        }
        #endregion

        #region [RegisterUserAsync]
        [Fact]
        [Trait("AuthService", "Register User")]
        public async Task RegisterUserAsync_ValidRole_UserCreatedAndAddedToRole()
        {
            var registrationDto = new UserRegistrationDto
            {
                UserName = "newuser",
                Password = "password",
                Role = "Admin"
            };
            var appUser = CreateTestUser(registrationDto.UserName);

            MockRoleManager.Setup(r => r.RoleExistsAsync(registrationDto.Role!)).ReturnsAsync(true);
            MockMapper.Setup(m => m.Map<ApplicationUser>(registrationDto)).Returns(appUser);
            MockUserManager.Setup(m => m.CreateAsync(appUser, registrationDto.Password)).ReturnsAsync(IdentityResult.Success);
            MockUserManager.Setup(m => m.AddToRoleAsync(appUser, registrationDto.Role!)).ReturnsAsync(IdentityResult.Success);

            var result = await _authService.RegisterUserAsync(registrationDto);

            Assert.True(result.Succeeded);
        }

        [Fact]
        [Trait("AuthService", "Register User")]
        public async Task RegisterUserAsync_InvalidRole_ReturnsFailed()
        {
            var registrationDto = new UserRegistrationDto
            {
                UserName = "newuser",
                Password = "password",
                Role = "NonExistentRole"
            };

            MockRoleManager.Setup(r => r.RoleExistsAsync(registrationDto.Role!)).ReturnsAsync(false);

            var result = await _authService.RegisterUserAsync(registrationDto);

            Assert.False(result.Succeeded);
            Assert.Contains(result.Errors, e => e.Description == "Role does not exist");
        }
        #endregion

        #region [CreateTokenAsync]
        [Fact]
        [Trait("AuthService", "Create Token")]
        public async Task CreateTokenAsync_WithAddTime_SetsRefreshTokenAndExpiry()
        {
            var appUser = CreateTestUser();
            MockUserManager.Setup(m => m.UpdateAsync(appUser)).ReturnsAsync(IdentityResult.Success);            
            MockUserManager.Setup(m => m.GetRolesAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(new List<string>());
            MockUserManager.Setup(m => m.FindByNameAsync(appUser.UserName!)).ReturnsAsync(appUser);
            MockUserManager.Setup(m => m.CheckPasswordAsync(appUser, "password")).ReturnsAsync(true);

            // First validate the user to set the internal user state
            await _authService.ValidateUserAsync(new UserAuthDto { UserName = appUser.UserName!, Password = "password" });

            var tokenDto = await _authService.CreateTokenAsync(addTime: true);

            Assert.NotNull(tokenDto.AccessToken);
            Assert.NotNull(tokenDto.RefreshToken);
        }

        [Fact]
        [Trait("AuthService", "Create Token")]
        public async Task CreateTokenAsync_UserNull_ThrowsArgumentNullException()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => _authService.CreateTokenAsync(false));
        }
        #endregion

        #region [RefreshTokenAsync]
        [Fact]
        [Trait("AuthService", "Refresh Token")]
        public async Task RefreshTokenAsync_ValidToken_ReturnsNewToken()
        {            
            var appUser = CreateTestUser();
            appUser.RefreshToken = "refresh123";
            appUser.RefreshTokenExpireTime = DateTime.UtcNow.AddDays(10); // Had to use AddDays to avoid timing issues
            
            
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtOptions.Value.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: JwtOptions.Value.Issuer,
                audience: JwtOptions.Value.Audience,
                claims: new[] { new Claim(ClaimTypes.Name, appUser.UserName!) },
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: creds
            );
            var accessToken = tokenHandler.WriteToken(token);

            
            MockUserManager.Setup(m => m.FindByNameAsync(appUser.UserName)).ReturnsAsync(appUser);
            MockUserManager.Setup(m => m.UpdateAsync(appUser)).ReturnsAsync(IdentityResult.Success);
            MockUserManager.Setup(m => m.GetRolesAsync(appUser)).ReturnsAsync(new List<string>());

            
            var userField = typeof(AuthService).GetField("user", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            userField!.SetValue(_authService, appUser);

            var oldRefreshToken = appUser.RefreshToken;
            var tokenDto = await _authService.CreateTokenAsync(addTime: true);

            
            Assert.NotNull(tokenDto.AccessToken);
            Assert.NotNull(tokenDto.RefreshToken);
            Assert.NotEqual(oldRefreshToken, tokenDto.RefreshToken);
        }

        [Fact]
        [Trait("AuthService", "Refresh Token")]
        public async Task RefreshTokenAsync_InvalidRefreshToken_ThrowsTokenValidationException()
        {
            var appUser = CreateTestUser();
            appUser.RefreshToken = "refresh123";
            appUser.RefreshTokenExpireTime = DateTime.UtcNow.AddMinutes(10);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtOptions.Value.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(issuer: JwtOptions.Value.Issuer, audience: JwtOptions.Value.Audience,
                claims: new[] { new Claim(ClaimTypes.Name, appUser.UserName!) },
                expires: DateTime.UtcNow.AddMinutes(10), signingCredentials: creds);
            var accessToken = tokenHandler.WriteToken(token);

            MockUserManager.Setup(m => m.FindByNameAsync(appUser.UserName)).ReturnsAsync(appUser);

            await Assert.ThrowsAsync<TokenValidationException>(() =>
                _authService.RefreshTokenAsync(new TokenDto(accessToken, "wrongtoken")));
        }

        [Fact]
        [Trait("AuthService", "Refresh Token")]
        public async Task RefreshTokenAsync_ExpiredRefreshToken_ThrowsTokenValidationException()
        {
            var appUser = CreateTestUser();
            appUser.RefreshToken = "refresh123";
            appUser.RefreshTokenExpireTime = DateTime.UtcNow.AddMinutes(-10);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtOptions.Value.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(issuer: JwtOptions.Value.Issuer, audience: JwtOptions.Value.Audience,
                claims: new[] { new Claim(ClaimTypes.Name, appUser.UserName!) },
                expires: DateTime.UtcNow.AddMinutes(10), signingCredentials: creds);
            var accessToken = tokenHandler.WriteToken(token);

            MockUserManager.Setup(m => m.FindByNameAsync(appUser.UserName)).ReturnsAsync(appUser);

            await Assert.ThrowsAsync<TokenValidationException>(() =>
                _authService.RefreshTokenAsync(new TokenDto(accessToken, appUser.RefreshToken!)));
        }

        [Fact]
        [Trait("AuthService", "Refresh Token")]
        public async Task RefreshTokenAsync_UserNotFound_ThrowsTokenValidationException()
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtOptions.Value.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(issuer: JwtOptions.Value.Issuer, audience: JwtOptions.Value.Audience,
                claims: new[] { new Claim(ClaimTypes.Name, "unknownuser") },
                expires: DateTime.UtcNow.AddMinutes(10), signingCredentials: creds);
            var accessToken = tokenHandler.WriteToken(token);

            MockUserManager.Setup(m => m.FindByNameAsync("unknownuser")).ReturnsAsync((ApplicationUser?)null);

            await Assert.ThrowsAsync<TokenValidationException>(() =>
                _authService.RefreshTokenAsync(new TokenDto(accessToken, "refresh123")));
        }
        #endregion
    }
}
