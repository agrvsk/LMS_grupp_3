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
}
