using Domain.Models.Entities;

namespace Domain.Contracts.Repositories;

public interface IApplicationUserRepository: IRepositoryBase<ApplicationUser>
{

    Task<ApplicationUser?> GetUserById(string userId);
    Task<List<ApplicationUser>> GetAllUsers();
    Task<List<string>> GetRolesInUser(ApplicationUser user);
    Task<List<ApplicationUser>> GetUsersByRole(string roleName);
    Task<List<ApplicationUser>> GetUsersByCourseId(string courseId);

}