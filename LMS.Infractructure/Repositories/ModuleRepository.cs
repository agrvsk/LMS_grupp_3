using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts.Repositories;
using Domain.Models.Entities;
using LMS.Infractructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infractructure.Repositories;

public class ModuleRepository : RepositoryBase<Module>, IModuleRepository
{
    private ApplicationDbContext context;

    public ModuleRepository(ApplicationDbContext context) : base(context)
    {
        this.context = context;
    }

    public async Task<List<Module>> GetAllModulesAsync()
    {
       
        return await FindAllAsync(trackChanges: false).ContinueWith(task => task.Result.ToList());
    }

    public async Task<Module?> GetModuleByIdAsync(Guid moduleId)
    {

        return await FindByConditionAsync(m => m.Id == moduleId, trackChanges: false)
            .ContinueWith(task => task.Result.SingleOrDefault());
    }

    public async Task<List<Module>> GetModulesByCourseIdAsync(Guid courseId)
    {
        
        return await FindByConditionAsync(m => m.CourseId == courseId, trackChanges: false)
            .ContinueWith(task => task.Result.ToList());
    }
    public async Task<List<Module>> GetModulesByCourseIdAndDateAsync(Guid courseId, DateTime idag)
    {
        var modules = await context.Modules
        .Where(m => m.CourseId == courseId &&
                idag.Date >= m.StartDate.Date &&
                idag.Date <= m.EndDate.Date)
        .Include(ma => ma.ModuleActivities)
        .AsNoTracking()
        .ToListAsync();

        // Filter subcollection
        foreach (var module in modules)
        {
            module.ModuleActivities = module.ModuleActivities
            .Where(mb => idag.Date >= mb.StartDate.Date && idag.Date <= mb.EndDate)
            .ToList();
        }
        return modules;
    }
    public async Task<List<Module>> GetAllModulesByDateAsync(DateTime idag)
    {
        var modules = await context.Modules
        .Where(m => idag.Date >= m.StartDate.Date &&
                    idag.Date <= m.EndDate.Date)
        .Include(m => m.ModuleActivities)
        .AsNoTracking()
        .ToListAsync();

        // Filter subcollection
        foreach (var module in modules)
        {
            module.ModuleActivities = module.ModuleActivities
            .Where(mb => idag.Date >= mb.StartDate.Date && idag.Date <= mb.EndDate)
            .ToList();
        }
        return modules;

    }


    public void CreateModule(Module module)
    {
        context.Modules.Add(module);
    }
    public void UpdateModule(Module module)
    {
        context.Modules.Update(module);
    }
    public void DeleteModule(Module module)
    {
        context.Modules.Remove(module);
    }
}
