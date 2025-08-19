using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts.Repositories;
using Domain.Models.Entities;
using Service.Contracts;

namespace LMS.Services;

public class CourseService : ICourseService
{
    private readonly IUnitOfWork uow;

    public CourseService(IUnitOfWork uow)
    {
        this.uow = uow;
    }


    public async Task<Course?> GetCourseByIdAsync(string courseId)
    {
        return (await uow.CourseRepository.GetCourseByIdAsync(courseId));
    }
    public async Task<List<Course>> GetAllCoursesAsync()
    {
        return (await uow.CourseRepository.GetAllCoursesAsync());
    }


}
