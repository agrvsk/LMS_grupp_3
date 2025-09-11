using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace LMS.Presentation.Controllers;

[Route("/schedules")]
[ApiController]
public class ScheduleController : ControllerBase
{
    private readonly IServiceManager _serviceManager;

    public ScheduleController(IServiceManager serviceManager)
    {
        _serviceManager = serviceManager;
    }
    [HttpGet("{courseId}")]
    public async Task<IActionResult> GetSchedule(Guid courseId )
    {
        
        var schedules =  await _serviceManager.ScheduleService.GetSchedule( courseId, DateTime.Today.AddMonths(-12), DateTime.Today.AddMonths(12));
        return Ok(schedules);
    }
    [HttpGet("{courseId}/{start}")]
    public async Task<IActionResult> GetScheduleByDate(Guid courseId, DateTime start)
    {
        var schedules = await _serviceManager.ScheduleService.GetSchedule(courseId, start, start.AddMonths(1));
        return Ok(schedules);
    }
}
