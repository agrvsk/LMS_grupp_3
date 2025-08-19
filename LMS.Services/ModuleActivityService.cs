using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts.Repositories;
using Domain.Models.Entities;
using Service.Contracts;

namespace LMS.Services;

public class ModuleActivityService : IModuleActivityService
{
    private readonly IUnitOfWork uow;

    public ModuleActivityService(IUnitOfWork uow)
    {
        this.uow = uow;
    }


    public async Task<ModuleActivity?> GetModuleActivityByIdAsync(string moduleActivityId)
    {
        return (await uow.ModuleActivityRepository.GetModuleActivityByIdAsync(moduleActivityId));
    }
    public async Task<List<ModuleActivity>> GetAllModuleActivitiesAsync()
    {
        return (await uow.ModuleActivityRepository.GetAllModuleActivitiesAsync());
    }
    public async Task<List<ModuleActivity>> GetModuleActivitiesByModuleIdAsync(string moduleId)
    {
        return (await uow.ModuleActivityRepository.GetModuleActivitiesByModuleIdAsync(moduleId));
    }


}
