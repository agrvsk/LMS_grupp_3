
using Domain.Models.Entities;

namespace Service.Contracts;

public interface IModuleActivityService
{
    Task<List<ModuleActivity>> GetAllModuleActivities();
    Task<List<ModuleActivity>> GetModuleActivitiesByModuleId(string moduleId);
    Task<ModuleActivity?> GetModuleActivityById(string moduleActivityId);
}