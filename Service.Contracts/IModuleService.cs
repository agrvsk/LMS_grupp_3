using Domain.Models.Entities;

namespace Service.Contracts;

public interface IModuleService
{
    Task<List<Module>> GetAllModulesAsync();
    Task<Module?> GetModuleByIdAsync(string moduleId);
    Task<List<Module>> GetModulesByCourseIdAsync(string courseId);
}