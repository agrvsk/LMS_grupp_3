using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts.Repositories;
using Domain.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Service.Contracts;

namespace LMS.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork uow;
    private readonly UserManager<ApplicationUser> userManager;

    public UserService(IUnitOfWork uow, UserManager<ApplicationUser> userManager)
    {
        uow = uow;
        userManager = userManager;
    }


    public async Task<ApplicationUser?> GetUserByIdAsync(string userId)
    {
        return await uow.ApplicationUserRepository.GetUserByIdAsync(userId);
    }
    public async Task<List<ApplicationUser>> GetAllUsersAsync()
    {
        return await uow.ApplicationUserRepository.GetAllUsersAsync();
    }

    public async Task<List<string>> GetRolesInUserAsync(ApplicationUser user)
    {
        return await uow.ApplicationUserRepository.GetRolesInUserAsync(user);

        //return await FindByConditionAsync(r =>   == roleName, trackChanges: false)
        //    .ContinueWith(task => task.Result.ToList());
        //context.Rol
        //var roles = await _userManager.GetUsersInRoleAsync("Teacher");
        //var role = await _userManager.GetRolesAsync(user);
        //return role.ToList();
        //if (role == null || !role.Any())
        //{
        //    return new List<ApplicationUser>();
        //}
        //return await FindByConditionAsync(u => u.Roles.Any(r => r.Name == roleName), trackChanges: false)
        //    .ContinueWith(task => task.Result.ToList());
    }
    public async Task<List<ApplicationUser>> GetUsersByRoleAsync(string roleName)
    {
        return await uow.ApplicationUserRepository.GetUsersByRoleAsync(roleName);
        //var usersInRole = await _userManager.GetUsersInRoleAsync(roleName);
        //return usersInRole.ToList();
    }

    public async Task<List<ApplicationUser>> GetUsersByCourseIdAsync(string courseId)
    {
        return await uow.ApplicationUserRepository.GetUsersByCourseIdAsync(courseId);
    }
}
