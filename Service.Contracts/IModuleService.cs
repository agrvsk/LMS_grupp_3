using Domain.Models.Entities;

namespace Service.Contracts;

public interface IModuleService
{
    Task<List<Module>> GetAllModulesAsync();
    Task<Module?> GetModuleById(string moduleId);
    Task<List<Module>> GetModulesByCourseId(string courseId);
}