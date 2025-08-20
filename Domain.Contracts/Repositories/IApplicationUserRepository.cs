using Domain.Models.Entities;

namespace Domain.Contracts.Repositories;

public interface IApplicationUserRepository: IRepositoryBase<ApplicationUser>
{

    Task<ApplicationUser?> GetUserByIdAsync(string userId);
    Task<List<ApplicationUser>> GetAllUsersAsync();
    Task<List<string>> GetRolesInUserAsync(ApplicationUser user);
    Task<List<ApplicationUser>> GetUsersByRoleAsync(string roleName);
    Task<List<ApplicationUser>> GetUsersByCourseIdAsync(Guid courseId);

}