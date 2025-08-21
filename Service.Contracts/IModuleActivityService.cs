
using Domain.Models.Entities;
using LMS.Shared.DTOs.EntityDto;

namespace Service.Contracts;

public interface IModuleActivityService
{
    Task<List<ModuleActivityDto>> GetAllModuleActivitiesAsync();
    Task<List<ModuleActivityDto>> GetModuleActivitiesByModuleIdAsync(Guid moduleId);
    Task<ModuleActivityDto?> GetModuleActivityByIdAsync(Guid moduleActivityId);
    Task<ModuleActivity> CreateActivityAsync(ModuleActivityCreateDto moduleActivity);
    Task<ModuleActivity> UpdateActivityAsync(ModuleActivityDto moduleActivity);
    Task<bool> DeleteActivityAsync(Guid moduleActivityId);
}