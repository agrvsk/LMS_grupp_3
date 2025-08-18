using Domain.Models.Entities;

namespace Domain.Contracts.Repositories;

public interface IModuleActivityRepository: IRepositoryBase<ModuleActivity>
{
    Task<ModuleActivity?> GetModuleActivityById(string moduleActivityId);
    Task<List<ModuleActivity>> GetAllModuleActivities();
    Task<List<ModuleActivity>> GetModuleActivitiesByModuleId(string moduleId);
}
