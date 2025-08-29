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
    private readonly ServiceManager sm;

    public ScheduleService(IUnitOfWork uow, )
    {
        this.uow = uow;
       
        // Initialize any required services or dependencies here
    }

    public ScheduleDto GetSchedule(Guid coursId, DateTime start, DateTime end)
    {
        throw new NotImplementedException();
    }
    //public ScheduleDto GetSchedule(Guid courseId, DateTime start, DateTime end)
    //{
    //    CourseDto course = sm.CourseService.GetCourseByIdAsync(courseId).Result;
    //    List<ModuleDto> modules = sm.ModuleService
    //        .GetModulesByCourseIdAsync(courseId).Result
    //        .Where(m => m.StartDate <= end && m.EndDate >= start)
    //        .ToList();
    //    List<ModuleActivityDto> moduleActivities = modules.SelectMany(m => m.ModuleActivities
    //    .Where(a => a.StartDate <= end && a.EndDate >= start))
    //        .ToList();
    //    return new ScheduleDto
    //    {
    //        StartDate = start,
    //        EndDate = end,
    //        Course = course,
    //        Modules = modules,
    //        ModuleActivities = moduleActivities
    //    };


    //}

}
