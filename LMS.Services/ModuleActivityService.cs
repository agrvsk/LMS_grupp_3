using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Contracts.Repositories;
using Domain.Models.Entities;
using LMS.Shared.DTOs.EntityDto;
using Service.Contracts;

namespace LMS.Services;

public class ModuleActivityService : IModuleActivityService
{
    private readonly IUnitOfWork uow;
    private readonly IMapper mapper;

    public ModuleActivityService(IUnitOfWork uow, IMapper mapper)
    {
        this.uow = uow;
        this.mapper = mapper;
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
    public async Task<ModuleActivity> CreateActivityAsync(ModuleActivityCreateDto moduleActivity)
    {
        var activity = mapper.Map<ModuleActivity>(moduleActivity);
        uow.ModuleActivityRepository.Create(activity);
        await uow.CompleteAsync();
        return activity;
    }
    public async Task<ModuleActivity> UpdateActivityAsync(ModuleActivityDto moduleActivity)
    {
        var existingActivity = await uow.ModuleActivityRepository.GetModuleActivityByIdAsync(moduleActivity.Id);
        if (existingActivity == null)
            return null;
        mapper.Map(moduleActivity, existingActivity);
        uow.ModuleActivityRepository.Update(existingActivity);
        await uow.CompleteAsync();
        return existingActivity;
    }
    public async Task<bool> DeleteActivityAsync(string moduleActivityId)
    {
        var activity = await uow.ModuleActivityRepository.GetModuleActivityByIdAsync(moduleActivityId);
        if (activity == null)
            return false;
        uow.ModuleActivityRepository.Delete(activity);
        await uow.CompleteAsync();
        return true;
    }


}
