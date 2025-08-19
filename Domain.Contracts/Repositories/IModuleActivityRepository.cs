using Domain.Models.Entities;

namespace Domain.Contracts.Repositories;

public interface IModuleActivityRepository: IRepositoryBase<ModuleActivity>
{
    Task<ModuleActivity?> GetModuleActivityByIdAsync(string moduleActivityId);
    Task<List<ModuleActivity>> GetAllModuleActivitiesAsync();
    Task<List<ModuleActivity>> GetModuleActivitiesByModuleIdAsync(string moduleId);
}
