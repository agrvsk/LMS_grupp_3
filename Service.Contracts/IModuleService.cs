using Domain.Models.Entities;
using LMS.Shared.DTOs.EntityDto;

namespace Service.Contracts;

public interface IModuleService
{
    Task<List<ModuleDto>> GetAllModulesAsync();
    Task<ModuleDto?> GetModuleByIdAsync(Guid moduleId);
    Task<List<ModuleDto>> GetModulesByCourseIdAsync(Guid courseId);
    Task<List<ModuleDto>> GetActivitiesByCourseIdAsync(Guid courseId, string idag);
    Task<List<ModuleDto>> GetAllActivitiesByDateAsync(string idag);
    Task<Module> CreateModuleAsync(ModuleCreateDto module);
    Task<ModuleDto> UpdateModuleAsync(ModuleUpdateDto module);
    Task<bool> DeleteModuleAsync(Guid moduleId);
}