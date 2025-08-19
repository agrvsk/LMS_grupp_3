using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts.Repositories;
using Domain.Models.Entities;
using Service.Contracts;

namespace LMS.Services;

public class ModuleService : IModuleService
{
    private readonly IUnitOfWork uow;

    public ModuleService(IUnitOfWork uow)
    {
        this.uow = uow;
    }


    public async Task<List<Module>> GetAllModulesAsync()
    {
        return await uow.ModuleRepository.GetAllModulesAsync();
    }

    public async Task<Module?> GetModuleByIdAsync(string moduleId)
    {
        return await uow.ModuleRepository.GetModuleByIdAsync(moduleId);
    }

    public async Task<List<Module>> GetModulesByCourseIdAsync(string courseId)
    {

        return await uow.ModuleRepository.GetModulesByCourseIdAsync(courseId);

    }
    public async Task<Module> CreateModuleAsync(Module module)
    {
        uow.ModuleRepository.Create(module);
        await uow.CompleteAsync();
        return module;
    }
    public async Task<Module> UpdateModuleAsync(Module module)
    {
        uow.ModuleRepository.Update(module);
        await uow.CompleteAsync();
        return module;
    }
    public async Task DeleteModuleAsync(string moduleId)
    {
        var module = await uow.ModuleRepository.GetModuleByIdAsync(moduleId);
        if (module != null)
        {
            uow.ModuleRepository.Delete(module);
            await uow.CompleteAsync();
        }
    }
}
