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
    [HttpGet("{coursId}")]
    public IActionResult GetSchedule(Guid coursId )
    {
        
        var schedules =  _serviceManager.ScheduleService.GetSchedule( coursId, DateTime.Today, DateTime.Today.AddMonths(1));
        return Ok(schedules);
    }
    [HttpGet("{coursId}/{start}")]
    public IActionResult GetScheduleByDate(Guid coursId, DateTime start)
    {
        var schedules = _serviceManager.ScheduleService.GetSchedule(coursId, start, start.AddMonths(1));
        return Ok(schedules);
    }
}
