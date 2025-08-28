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
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid CourseId { get; set; }

        public List<ModuleActivityCreateDto> ModuleActivities { get; set; } = new List<ModuleActivityCreateDto>();
    }
}
