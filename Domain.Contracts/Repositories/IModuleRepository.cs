using Domain.Models.Entities;
namespace Domain.Contracts.Repositories;

public interface IModuleRepository: IRepositoryBase<Module>
{

    Task<Module?> GetModuleByIdAsync(string moduleId);
    Task<List<Module>> GetAllModulesAsync();
    Task<List<Module>> GetModulesByCourseIdAsync(string courseId);
}
