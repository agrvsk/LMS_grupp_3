using Domain.Models.Entities;

namespace Service.Contracts;

public interface IUserService
{
    Task<List<ApplicationUser>> GetAllUsersAsync();
    Task<List<string>> GetRolesInUserAsync(ApplicationUser user);
    Task<ApplicationUser?> GetUserByIdAsync(string userId);
    Task<List<ApplicationUser>> GetUsersByCourseIdAsync(string courseId);
    Task<List<ApplicationUser>> GetUsersByRoleAsync(string roleName);
}