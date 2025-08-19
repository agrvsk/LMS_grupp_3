using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts.Repositories;
using Domain.Models.Entities;
using LMS.Infractructure.Data;

namespace LMS.Infractructure.Repositories;

public class ModuleRepository : RepositoryBase<Module>, IModuleRepository
{
    public ModuleRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<List<Module>> GetAllModulesAsync()
    {
       
        return await FindAllAsync(trackChanges: false).ContinueWith(task => task.Result.ToList());
    }

    public async Task<Module?> GetModuleByIdAsync(string moduleId)
    {

        return await FindByConditionAsync(m => m.Id == moduleId, trackChanges: false)
            .ContinueWith(task => task.Result.SingleOrDefault());
    }

    public async Task<List<Module>> GetModulesByCourseIdAsync(string courseId)
    {
        
        return await FindByConditionAsync(m => m.CourseId == courseId, trackChanges: false)
            .ContinueWith(task => task.Result.ToList());
    }
}
