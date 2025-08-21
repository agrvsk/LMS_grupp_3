using Domain.Contracts.Repositories;
using LMS.Shared.DTOs.EntityDto;
using LMS.Shared.DTOs.EntityDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Services;

public class ScheduleService
{
    private readonly IUnitOfWork uow;
    private readonly ServiceManager sm;

    public ScheduleService(IUnitOfWork uow, ServiceManager sm)
    {
        this.uow = uow;
        this.sm = sm;
        // Initialize any required services or dependencies here
    }
    public ScheduleDto GetSchedule(Guid coursId, DateTime start, DateTime end)
    {
        CourseDto course = sm.CourseService.GetCourseByIdAsync(coursId).Result;
        List<ModuleDto> modules = sm.ModuleService.GetModulesByCourseIdAsync(coursId).Result.Where(m => m.EndDate <= start && m.StartDate >= end).ToList();
        List<ModuleActivityDto> moduleActivities = modules.SelectMany(m => m.ModuleActivities.Where(a => a.StartDate <= start && a.EndDate >= end)).ToList();
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
