using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Contracts.Repositories;
using Domain.Models.Entities;
using LMS.Shared.DTOs.EntityDto;
using Microsoft.AspNetCore.Identity;
using Service.Contracts;

namespace LMS.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork uow;
    private readonly UserManager<ApplicationUser> userManager;
    private readonly IMapper mapper;

    public UserService(IUnitOfWork uow, UserManager<ApplicationUser> userManager, IMapper mapper)
    {
        this.uow = uow;
        this.userManager = userManager;
        this.mapper = mapper;
    }


    public async Task<UserDto?> GetUserByIdAsync(string userId)
    {
        var user = await uow.ApplicationUserRepository.GetUserByIdAsync(userId);
        var userDto = mapper.Map<UserDto>(user);
        return userDto;
    }
    public async Task<List<UserDto>> GetAllUsersAsync()
    {
        var users = await uow.ApplicationUserRepository.GetAllUsersAsync();
        var userDtos = mapper.Map<List<UserDto>>(users);
        return userDtos;
    }

    public async Task<List<string>> GetRolesInUserAsync(ApplicationUser user)
    {
        return await uow.ApplicationUserRepository.GetRolesInUserAsync(user);
    }
    public async Task<List<UserDto>> GetUsersByRoleAsync(string roleName)
    {
        var users = await userManager.GetUsersInRoleAsync(roleName);
        var userDtos = mapper.Map<List<UserDto>>(users);
        return userDtos;
    }

    public async Task<List<UserDto>> GetUsersByCourseIdAsync(Guid courseId)
    {
        var users = await uow.ApplicationUserRepository.GetUsersByCourseIdAsync(courseId);
        var userDtos = mapper.Map<List<UserDto>>(users);
        return userDtos;
    }
}
