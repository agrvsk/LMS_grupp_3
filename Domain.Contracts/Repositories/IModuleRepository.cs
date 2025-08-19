using Domain.Models.Entities;
namespace Domain.Contracts.Repositories;

public interface IModuleRepository: IRepositoryBase<Module>
{

    Task<Module?> GetModuleById(string moduleId);
    Task<List<Module>> GetAllModulesAsync();
    Task<List<Module>> GetModulesByCourseId(string courseId);
}
