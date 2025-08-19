using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Contracts.Repositories;
using Domain.Models.Entities;
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


    public async Task<Course?> GetCourseByIdAsync(string courseId)
    {
        return (await uow.CourseRepository.GetCourseByIdAsync(courseId));
    }
    public async Task<List<Course>> GetAllCoursesAsync()
    {
        return (await uow.CourseRepository.GetAllCoursesAsync());
    }
    public async Task<Course> CreateCourseAsync(CourseCreateDto courseDto)
    {
        var course= mapper.Map<Course>(courseDto);
        uow.CourseRepository.Create(course);
        await uow.CompleteAsync();
        return course;
    }
    public async Task<Course> UpdateCourseAsync(CourseDto courseDto)
    {
        var course = mapper.Map<Course>(courseDto);
        uow.CourseRepository.Update(course);
        await uow.CompleteAsync();
        return course;
    }
    public async Task<bool> DeleteCourseAsync(string courseId)
    {
        var course = await uow.CourseRepository.GetCourseByIdAsync(courseId);
        if (course == null)
            return false;
        uow.CourseRepository.Delete(course);
        await uow.CompleteAsync();
        return true;
    }


}
