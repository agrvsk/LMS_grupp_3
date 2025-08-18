using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts.Repositories;
using Domain.Models.Entities;
using LMS.Infractructure.Data;

namespace LMS.Infractructure.Repositories;

public class ModuleActivityRepository: RepositoryBase<ModuleActivity>, IModuleActivityRepository
{
    public ModuleActivityRepository(ApplicationDbContext context) : base(context)
    {
    }
    public async Task<ModuleActivity?> GetModuleActivityById(string moduleActivityId)
    {
        return (await FindByConditionAsync(ma => ma.Id == moduleActivityId, trackChanges: false)).SingleOrDefault();
    }
    public async Task<List<ModuleActivity>> GetAllModuleActivities()
    {
        return (await FindAllAsync(trackChanges: false)).ToList();
    }
    public async Task<List<ModuleActivity>> GetModuleActivitiesByModuleId(string moduleId)
    {
        return (await FindByConditionAsync(ma => ma.ModuleId == moduleId, trackChanges: false)).ToList();
    }
}
