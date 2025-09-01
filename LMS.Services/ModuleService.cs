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

public class ModuleService : IModuleService
{
    private readonly IUnitOfWork uow;
    private readonly IMapper mapper;

    public ModuleService(IUnitOfWork uow, IMapper mapper)
    {
        this.uow = uow;
        this.mapper = mapper;
    }


    public async Task<List<ModuleDto>> GetAllModulesAsync()
    {
        var modules = await uow.ModuleRepository.GetAllModulesAsync();
        var moduleDtos = mapper.Map<List<ModuleDto>>(modules);
        return moduleDtos;
    }

    public async Task<ModuleDto?> GetModuleByIdAsync(Guid moduleId)
    {
        var module = await uow.ModuleRepository.GetModuleByIdAsync(moduleId);
        var moduleDto = mapper.Map<ModuleDto>(module);
        return moduleDto;
    }

    public async Task<List<ModuleDto>> GetModulesByCourseIdAsync(Guid courseId)
    {

        return mapper.Map<List<ModuleDto>>(await uow.ModuleRepository.GetModulesByCourseIdAsync(courseId));

    }
    public async Task<List<ModuleDto>> GetActivitiesByCourseIdAsync(Guid courseId, string idag)
    {
        return mapper.Map<List<ModuleDto>>(await uow.ModuleRepository.GetModulesByCourseIdAndDateAsync(courseId,idag));
    }
    public async Task<List<ModuleDto>> GetAllActivitiesByDateAsync(string idag)
    {
        return mapper.Map<List<ModuleDto>>(await uow.ModuleRepository.GetAllModulesByDateAsync(idag));
    }

    public async Task<Module> CreateModuleAsync(ModuleCreateDto moduleDto)
    {
        var module = mapper.Map<Module>(moduleDto);
        uow.ModuleRepository.Create(module);
        await uow.CompleteAsync();
        return module;
    }
    public async Task<ModuleDto> UpdateModuleAsync(ModuleUpdateDto moduleupdateDto)
    {
        var module = mapper.Map<Module>(moduleupdateDto);
        uow.ModuleRepository.Update(module);
        await uow.CompleteAsync();
        var moduleDto = mapper.Map<ModuleDto>(module);
        return moduleDto;
    }
    public async Task<bool> DeleteModuleAsync(Guid moduleId)
    {
        var module = await uow.ModuleRepository.GetModuleByIdAsync(moduleId);
        if (module != null)
        {
            uow.ModuleRepository.Delete(module);
            await uow.CompleteAsync();
            return true;
        }
        return false;
    }
}
