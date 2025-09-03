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

public class ModuleActivityRepository : RepositoryBase<ModuleActivity>, IModuleActivityRepository
{
    private readonly ApplicationDbContext _context;
    public ModuleActivityRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }
    public async Task<ModuleActivity?> GetModuleActivityByIdAsync(Guid moduleActivityId)
    {
        return await _context.Activities
            .Include(ma => ma.Assignments)
            .AsNoTracking()
            .FirstOrDefaultAsync(ma => ma.Id == moduleActivityId);
    }
    public async Task<List<ModuleActivity>> GetAllModuleActivitiesAsync()
    {
        return await _context.Activities
            .Include(ma => ma.Assignments)
            .AsNoTracking()
            .ToListAsync();
    }
    public async Task<List<ModuleActivity>> GetModuleActivitiesByModuleIdAsync(Guid moduleId)
    {
        return await _context.Activities
        .Include(ma => ma.Assignments)
        .Where(ma => ma.ModuleId == moduleId)
        .AsNoTracking()
        .ToListAsync();
    }
}
