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


    public async Task<ModuleActivity?> GetModuleActivityById(string moduleActivityId)
    {
        return (await uow.ModuleActivityRepository.GetModuleActivityById(moduleActivityId));
    }
    public async Task<List<ModuleActivity>> GetAllModuleActivities()
    {
        return (await uow.ModuleActivityRepository.GetAllModuleActivities());
    }
    public async Task<List<ModuleActivity>> GetModuleActivitiesByModuleId(string moduleId)
    {
        return (await uow.ModuleActivityRepository.GetModuleActivitiesByModuleId(moduleId));
    }


}
