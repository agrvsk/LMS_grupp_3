using Domain.Contracts.Repositories;
using LMS.Shared.DTOs.EntityDto;
using LMS.Shared.DTOs.EntityDTO;
using Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Services;

public class ScheduleService : IScheduleService
{
    private readonly IUnitOfWork uow;
    private readonly ICourseService courseService;
    private readonly IModuleService moduleService;
   

    public ScheduleService(IUnitOfWork uow,ICourseService courseService, IModuleService moduleService )
    {
        this.uow = uow;
        this.courseService = courseService;
        this.moduleService = moduleService;

        // Initialize any required services or dependencies here
    }

    //public ScheduleDto GetSchedule(Guid coursId, DateTime start, DateTime end)
    //{
    //    throw new NotImplementedException();
    //}
    public ScheduleDto GetSchedule(Guid courseId, DateTime start, DateTime end)
    {
        CourseDto course = courseService.GetCourseByIdAsync(courseId).Result;
        List<ModuleDto> modules = moduleService
            .GetModulesByCourseIdAsync(courseId).Result
            .Where(m => m.StartDate <= end && m.EndDate >= start)
            .ToList();
        List<ModuleActivityDto> moduleActivities = modules.SelectMany(m => m.ModuleActivities
        .Where(a => a.StartDate <= end && a.EndDate >= start))
            .ToList();
        return new ScheduleDto
        {
            StartDate = start,
            EndDate = end,
            Course = course,
            Modules = modules,
            ModuleActivities = moduleActivities
        };


    }

}
