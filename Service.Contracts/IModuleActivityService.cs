
using Domain.Models.Entities;

namespace Service.Contracts;

public interface IModuleActivityService
{
    Task<List<ModuleActivity>> GetAllModuleActivitiesAsync();
    Task<List<ModuleActivity>> GetModuleActivitiesByModuleIdAsync(string moduleId);
    Task<ModuleActivity?> GetModuleActivityByIdAsync(string moduleActivityId);
}