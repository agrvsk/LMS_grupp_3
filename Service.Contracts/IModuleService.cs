using Domain.Models.Entities;
using LMS.Shared.DTOs.EntityDto;

namespace Service.Contracts;

public interface IModuleService
{
    Task<List<Module>> GetAllModulesAsync();
    Task<Module?> GetModuleByIdAsync(Guid moduleId);
    Task<List<Module>> GetModulesByCourseIdAsync(Guid courseId);
    Task<Module> CreateModuleAsync(ModuleCreateDto module);
    Task<Module> UpdateModuleAsync(ModuleDto module);
    Task<bool> DeleteModuleAsync(Guid moduleId);
}