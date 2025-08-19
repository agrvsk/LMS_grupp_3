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


    public async Task<ApplicationUser?> GetUserById(string userId)
    {
        return await GetUserById(userId);
    }
    public async Task<List<ApplicationUser>> GetAllUsers()
    {
        return await GetAllUsers();
    }

    public async Task<List<string>> GetRolesInUser(ApplicationUser user)
    {
        return await uow.ApplicationUserRepository.GetRolesInUser(user);

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
    public async Task<List<ApplicationUser>> GetUsersByRole(string roleName)
    {
        return await uow.ApplicationUserRepository.GetUsersByRole(roleName);
        //var usersInRole = await _userManager.GetUsersInRoleAsync(roleName);
        //return usersInRole.ToList();
    }

    public async Task<List<ApplicationUser>> GetUsersByCourseId(string courseId)
    {
        return await uow.ApplicationUserRepository.GetUsersByCourseId(courseId);
    }
}
