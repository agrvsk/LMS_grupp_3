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
    //public ScheduleDto GetSchedule(Guid coursId, DateTime start, DateTime end) {
    
    //        CourseDto course = sm.CourseService.GetCourseByIdAsync(coursId).Result;

    //}

}
