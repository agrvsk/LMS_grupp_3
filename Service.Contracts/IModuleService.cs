using Domain.Models.Entities;
using LMS.Shared.DTOs.EntityDto;

namespace Service.Contracts;

public interface IModuleService
{
    Task<List<Module>> GetAllModulesAsync();
    Task<Module?> GetModuleByIdAsync(string moduleId);
    Task<List<Module>> GetModulesByCourseIdAsync(string courseId);
    Task<Module> CreateModuleAsync(ModuleCreateDto module);
    Task<Module> UpdateModuleAsync(ModuleDto module);
    Task<bool> DeleteModuleAsync(string moduleId);
}