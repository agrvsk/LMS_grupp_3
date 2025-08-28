using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Shared.DTOs.EntityDto
{
    public record CourseCreateDto
    {
        [MaxLength(100)]
        [Required(ErrorMessage = "Please enter a course name")]
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime StartDate { get; set; } = DateTime.Today;
        public DateTime EndDate { get; set; } = DateTime.Today.AddMonths(3);
        public List<ModuleCreateDto> Modules { get; set; } = new List<ModuleCreateDto>();
    }
}
