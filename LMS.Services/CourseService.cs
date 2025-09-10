using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Contracts.Repositories;
using Domain.Models.Entities;
using Domain.Models.Exceptions;
using LMS.Shared.DTOs.EntityDto;
using Service.Contracts;

namespace LMS.Services;

public class CourseService : ICourseService
{
    private readonly IUnitOfWork uow;
    private readonly IMapper mapper;

    public CourseService(IUnitOfWork uow, IMapper mapper)
    {
        this.uow = uow;
        this.mapper = mapper;
    }


    public async Task<CourseDto?> GetCourseByIdAsync(Guid courseId)
    {
        var course = await uow.CourseRepository.GetCourseByIdAsync(courseId);
        if (course == null) throw new CourseNotFoundException(courseId);
        return mapper.Map<CourseDto>(course);
    }
    public async Task<List<CourseDto>> GetAllCoursesAsync()
    {
        var courses = await uow.CourseRepository.GetAllCoursesAsync();
        var courseDtos = mapper.Map<List<CourseDto>>(courses);
        return courseDtos.OrderBy(c=> c.StartDate).ToList();
    }
    public async Task<List<AssignmentDto>> GetAssignmentsByCourseIdAsync(Guid courseId)
    {
        var modules = await uow.ModuleRepository.GetModulesByCourseIdAsync(courseId);
        var allAssignments = new List<Assignment>();
        foreach (var mod in modules)
        {
            var activities = await uow.ModuleActivityRepository.GetModuleActivitiesByModuleIdAsync(mod.Id);
            foreach (var act in activities)
            {
                var assignments = await uow.ModuleActivityRepository.GetAssignmentsByModuleActivityIdAsync(act.Id);
                allAssignments.AddRange(assignments);
            }
        }
        var assignmentDtos = mapper.Map<List<AssignmentDto>>(allAssignments);
        return assignmentDtos.OrderBy(c => c.DueDate).ToList();
    }
    public async Task<CourseDto> CreateCourseAsync(CourseCreateDto courseDto)
    {
        var course= mapper.Map<Course>(courseDto);
        uow.CourseRepository.Create(course);
        var newCourseDto = mapper.Map<CourseDto>(course);
        await uow.CompleteAsync();

        return newCourseDto;
    }
    public async Task<CourseDto> UpdateCourseAsync(CourseUpdateDto courseUpdDto)
    {
        var course = mapper.Map<Course>(courseUpdDto);
        uow.CourseRepository.Update(course);
        await uow.CompleteAsync();
        return mapper.Map<CourseDto>(course);
//        return true;
    }
    public async Task<bool> DeleteCourseAsync(Guid courseId)
    {
        var course = await uow.CourseRepository.GetCourseByIdAsync(courseId);
        if (course == null)
            return false;

        var documents = await uow.DocumentRepository.GetDocumentsByParentAsync(courseId, "course");
        foreach (var doc in documents)
        {
            uow.DocumentRepository.Delete(doc);
        }

        var comrades = await uow.ApplicationUserRepository.GetUsersByCourseIdAsync(course.Id);

        foreach (var ussr in comrades)
        {            
            uow.ApplicationUserRepository.Delete(ussr);
        }

        var modules = await uow.ModuleRepository.GetModulesByCourseIdAsync(courseId);


        foreach (var mod in modules)
        {
            var activities = await uow.ModuleActivityRepository.GetModuleActivitiesByModuleIdAsync(mod.Id);

            foreach (var act in activities)
            {
                uow.ModuleActivityRepository.Delete(act);
            }

            uow.ModuleRepository.Delete(mod);
        }


        uow.CourseRepository.Delete(course);
        await uow.CompleteAsync();
        return true;
    }


}
