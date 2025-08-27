using Domain.Models.Entities;
namespace Domain.Contracts.Repositories;

public interface IModuleRepository: IRepositoryBase<Module>
{

    Task<Module?> GetModuleByIdAsync(Guid moduleId);
    Task<List<Module>> GetAllModulesAsync();
    Task<List<Module>> GetModulesByCourseIdAsync(Guid courseId);
    Task<List<Module>> GetModulesByCourseIdAndDateAsync(Guid courseId, DateTime idag);
    Task<List<Module>> GetAllModulesByDateAsync(DateTime idag);

    void CreateModule(Module module);
    void UpdateModule(Module module);
    void DeleteModule(Module moduleId);
}
