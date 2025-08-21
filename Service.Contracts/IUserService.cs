using Domain.Models.Entities;
using LMS.Shared.DTOs.EntityDto;

namespace Service.Contracts;

public interface IUserService
{
    Task<List<UserDto>> GetAllUsersAsync();
    Task<List<string>> GetRolesInUserAsync(ApplicationUser user);
    Task<UserDto?> GetUserByIdAsync(string userId);
    Task<List<UserDto>> GetUsersByCourseIdAsync(Guid courseId);
    Task<List<UserDto>> GetUsersByRoleAsync(string roleName);
}