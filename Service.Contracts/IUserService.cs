using Domain.Models.Entities;

namespace Service.Contracts;

public interface IUserService
{
    Task<List<ApplicationUser>> GetAllUsers();
    Task<List<string>> GetRolesInUser(ApplicationUser user);
    Task<ApplicationUser?> GetUserById(string userId);
    Task<List<ApplicationUser>> GetUsersByCourseId(string courseId);
    Task<List<ApplicationUser>> GetUsersByRole(string roleName);
}