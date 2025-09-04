
using Domain.Models.Entities;
using LMS.Shared.DTOs.EntityDto;
using Microsoft.AspNetCore.Http;

namespace Service.Contracts;

public interface IModuleActivityService
{
    Task<List<ModuleActivityDto>> GetAllModuleActivitiesAsync();
    Task<List<ModuleActivityDto>> GetModuleActivitiesByModuleIdAsync(Guid moduleId);
    Task<ModuleActivityDto?> GetModuleActivityByIdAsync(Guid moduleActivityId);
    Task<ModuleActivity> CreateActivityAsync(ModuleActivityCreateDto moduleActivity);
    Task<ModuleActivityDto> CreateActivityWithDocumentsAsync(ModuleActivityCreateDto dto, List<IFormFile> files);
    Task<ModuleActivityDto> UpdateActivityAsync(ModuleActivityUpdateDto moduleActivity);
    Task<bool> DeleteActivityAsync(Guid moduleActivityId);
}