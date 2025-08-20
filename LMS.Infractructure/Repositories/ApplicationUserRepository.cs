using Domain.Contracts.Repositories;
using Domain.Models.Entities;
using LMS.Infractructure.Data;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LMS.Infractructure.Repositories;

public class ApplicationUserRepository:RepositoryBase<ApplicationUser>, IApplicationUserRepository
{
    private readonly ApplicationDbContext context;
    private readonly Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> _userManager;
    public ApplicationUserRepository(ApplicationDbContext context, Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager) : base(context)
    {
        this.context = context;
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    }
    public async Task<ApplicationUser?> GetUserByIdAsync(string userId)
    {
        return await FindByConditionAsync(u => u.Id == userId, trackChanges: false)
            .ContinueWith(task => task.Result.SingleOrDefault());
    }
    public async Task<List<ApplicationUser>> GetAllUsersAsync()
    {
        return await FindAllAsync(trackChanges: false).ContinueWith(task => task.Result.ToList());
    }

    public async Task<List<string>> GetRolesInUserAsync(ApplicationUser user)
    {
        //return await FindByConditionAsync(r =>   == roleName, trackChanges: false)
        //    .ContinueWith(task => task.Result.ToList());
        //context.Rol
        //var roles = await _userManager.GetUsersInRoleAsync("Teacher");
        var role = await _userManager.GetRolesAsync(user);
        return role.ToList();
        //if (role == null || !role.Any())
        //{
        //    return new List<ApplicationUser>();
        //}
        //return await FindByConditionAsync(u => u.Roles.Any(r => r.Name == roleName), trackChanges: false)
        //    .ContinueWith(task => task.Result.ToList());
    }
    public async Task<List<ApplicationUser>> GetUsersByRoleAsync(string roleName)
    {
        var usersInRole = await _userManager.GetUsersInRoleAsync(roleName);
        return usersInRole.ToList();
    }

    public async Task<List<ApplicationUser>> GetUsersByCourseIdAsync(Guid courseId)
    {
        return await FindByConditionAsync(c => c.CourseId == courseId, trackChanges: false)
            .ContinueWith(task => task.Result.ToList());
    }
}
