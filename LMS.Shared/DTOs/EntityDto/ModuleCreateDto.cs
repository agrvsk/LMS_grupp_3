using LMS.Shared.DTOs.EntityDto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Shared.DTOs.EntityDto
{
    public record ModuleCreateDto
    {
        [MaxLength(100)]
        [Required(ErrorMessage ="Please enter a module name")]
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime StartDate { get; set; } = DateTime.Today;
        public DateTime EndDate { get; set; } = DateTime.Today.AddMonths(1);
        [Required]
        public Guid CourseId { get; set; }

        public List<ModuleActivityCreateDto> ModuleActivities { get; set; } = new List<ModuleActivityCreateDto>();
    }
}
