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
    private readonly ApplicationDbContext _context;
    public ModuleActivityRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }
    public async Task<ModuleActivity?> GetModuleActivityByIdAsync(Guid moduleActivityId)
    {
        return (await FindByConditionAsync(ma => ma.Id == moduleActivityId, trackChanges: false)).SingleOrDefault();
    }
    public async Task<List<ModuleActivity>> GetAllModuleActivitiesAsync()
    {
        return (await FindAllAsync(trackChanges: false)).ToList();
    }
    public async Task<List<ModuleActivity>> GetModuleActivitiesByModuleIdAsync(Guid moduleId)
    {
        return (await FindByConditionAsync(ma => ma.ModuleId == moduleId, trackChanges: false)).ToList();
    }
}
