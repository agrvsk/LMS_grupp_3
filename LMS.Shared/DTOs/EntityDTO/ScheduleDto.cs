using LMS.Shared.DTOs.EntityDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Shared.DTOs.EntityDTO;

public record ScheduleDto
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public CourseDto Course { get; set; }
    public List<ModuleDto> Modules { get; set; } 

    public List<ModuleActivityDto> ModuleActivities { get; set; } 
}
