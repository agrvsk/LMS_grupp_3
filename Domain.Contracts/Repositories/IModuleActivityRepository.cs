using Domain.Models.Entities;

namespace Domain.Contracts.Repositories;

public interface IModuleActivityRepository: IRepositoryBase<ModuleActivity>
{
    Task<ModuleActivity?> GetModuleActivityByIdAsync(Guid moduleActivityId);
    Task<List<ModuleActivity>> GetAllModuleActivitiesAsync();
    Task<List<ModuleActivity>> GetModuleActivitiesByModuleIdAsync(Guid moduleId);
    Task<List<Assignment>> GetAssignmentsByModuleActivityIdAsync(Guid moduleActivityId);
}
