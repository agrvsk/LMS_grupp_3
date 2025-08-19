
using Domain.Models.Entities;
using LMS.Shared.DTOs.EntityDto;

namespace Service.Contracts;

public interface IModuleActivityService
{
    Task<List<ModuleActivity>> GetAllModuleActivitiesAsync();
    Task<List<ModuleActivity>> GetModuleActivitiesByModuleIdAsync(string moduleId);
    Task<ModuleActivity?> GetModuleActivityByIdAsync(string moduleActivityId);
    Task<ModuleActivity> CreateActivityAsync(ModuleActivityCreateDto moduleActivity);
    Task<ModuleActivity> UpdateActivityAsync(ModuleActivityDto moduleActivity);
    Task<bool> DeleteActivityAsync(string moduleActivityId);
}