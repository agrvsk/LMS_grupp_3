using Domain.Models.Entities;
using Domain.Models.Exceptions;
using LMS.Services;
using LMS.Shared.DTOs.EntityDto;
using LMS.UnitTests.Setups;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.UnitTests.Services
{
    public class UserServiceTests : ServiceTestBase
    {
        private readonly UserService _service;

        public UserServiceTests()
        {
            _service = new UserService(MockUow.Object, MockUserManager.Object, MockMapper.Object);
        }

        [Fact]
        [Trait("UserService", "GetUserById")]
        public async Task GetUserByIdAsync_UserExists_ReturnsUserDto()
        {
            var userId = "abc123";
            var user = new ApplicationUser { Id = userId, UserName = "TestUser", Email = "test@user.com" };
            var userDto = new UserDto { Name = "TestUser", Email = "test@user.com", Role = "Student" };

            MockUow.Setup(u => u.ApplicationUserRepository.GetUserByIdAsync(userId))
                   .ReturnsAsync(user);

            MockMapper.Setup(m => m.Map<UserDto>(user))
                      .Returns(userDto);

            var result = await _service.GetUserByIdAsync(userId);

            Assert.NotNull(result);
            Assert.Equal("TestUser", result.Name);
            Assert.Equal("test@user.com", result.Email);
        }

        [Fact]
        [Trait("UserService", "GetUserById")]
        public async Task GetUserByIdAsync_UserDoesNotExist_ThrowsException()
        {
            var userId = "usertest";

            MockUow.Setup(u => u.ApplicationUserRepository.GetUserByIdAsync(userId))
                   .ReturnsAsync((ApplicationUser?)null);

            await Assert.ThrowsAsync<UserNotFoundException>(async () =>
            {
                await _service.GetUserByIdAsync(userId);
            });
        }

        [Fact]
        [Trait("UserService", "GetAllUsers")]
        public async Task GetAllUsersAsync_UsersExist_ReturnsUserDtos()
        {
            var users = new List<ApplicationUser>
            {
                new ApplicationUser { Id = "1", UserName = "User1", Email = "user1@test.com" }
            };

            var userDtos = new List<UserDto>
            {
                new UserDto { Name = "User1", Email = "user1@test.com", Role = "Student" }
            };

            MockUow.Setup(u => u.ApplicationUserRepository.GetAllUsersAsync())
                   .ReturnsAsync(users);

            MockMapper.Setup(m => m.Map<List<UserDto>>(users))
                      .Returns(userDtos);

            var result = await _service.GetAllUsersAsync();

            Assert.Single(result);
            Assert.Equal("User1", result.First().Name);
        }

        [Fact]
        [Trait("UserService", "GetAllUsers")]
        public async Task GetAllUsersAsync_NoUsers_ReturnsEmptyList()
        {
            MockUow.Setup(u => u.ApplicationUserRepository.GetAllUsersAsync())
                   .ReturnsAsync(new List<ApplicationUser>());

            MockMapper.Setup(m => m.Map<List<UserDto>>(It.IsAny<List<ApplicationUser>>()))
                      .Returns(new List<UserDto>());

            var result = await _service.GetAllUsersAsync();

            Assert.Empty(result);
        }

        [Fact]
        [Trait("UserService", "GetRolesInUser")]
        public async Task GetRolesInUserAsync_UserHasRoles_ReturnsRoles()
        {
            var user = new ApplicationUser { Id = "123", UserName = "RoleUser" };
            var roles = new List<string> { "Student", "Teacher" };

            MockUow.Setup(u => u.ApplicationUserRepository.GetRolesInUserAsync(user))
                   .ReturnsAsync(roles);

            var result = await _service.GetRolesInUserAsync(user);

            Assert.Equal(2, result.Count);
            Assert.Contains("Student", result);
        }

        [Fact]
        [Trait("UserService", "GetUsersByRole")]
        public async Task GetUsersByRoleAsync_UsersExist_ReturnsUserDtos()
        {
            var users = new List<ApplicationUser>
            {
                new ApplicationUser { Id = "1", UserName = "RoleUser1", Email = "role1@test.com" }
            };

            var userDtos = new List<UserDto>
            {
                new UserDto { Name = "RoleUser1", Email = "role1@test.com", Role = "Teacher" }
            };

            MockUserManager.Setup(m => m.GetUsersInRoleAsync("Teacher"))
                           .ReturnsAsync(users);

            MockMapper.Setup(m => m.Map<List<UserDto>>(users))
                      .Returns(userDtos);

            var result = await _service.GetUsersByRoleAsync("Teacher");

            Assert.Single(result);
            Assert.Equal("RoleUser1", result.First().Name);
        }

        [Fact]
        [Trait("UserService", "GetUsersByCourseId")]
        public async Task GetUsersByCourseIdAsync_UsersExist_ReturnsUserDtos()
        {
            var courseId = Guid.NewGuid();

            var users = new List<ApplicationUser>
            {
                new ApplicationUser { Id = "1", UserName = "CourseUser", Email = "courseuser@test.com" }
            };

            var userDtos = new List<UserDto>
            {
                new UserDto { Name = "CourseUser", Email = "courseuser@test.com", Role = "Student" }
            };

            MockUow.Setup(u => u.ApplicationUserRepository.GetUsersByCourseIdAsync(courseId))
                   .ReturnsAsync(users);

            MockMapper.Setup(m => m.Map<List<UserDto>>(users))
                      .Returns(userDtos);

            var result = await _service.GetUsersByCourseIdAsync(courseId);

            Assert.Single(result);
            Assert.Equal("CourseUser", result.First().Name);
        }        
    }
}