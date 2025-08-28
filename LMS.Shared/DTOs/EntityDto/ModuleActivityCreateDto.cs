using Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Shared.DTOs.EntityDto
{
    public record ModuleActivityCreateDto
    {
        [MaxLength(100)]
        [MinLength(1)]
        [Required(ErrorMessage = "Please enter an activity name")]
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime StartDate { get; set; } = DateTime.Today.AddHours(9);
        public DateTime EndDate { get; set; } = DateTime.Today.AddHours(17);
        [Required(ErrorMessage = "Please select an activity type")]
        public int? ActivityTypeId { get; set; }
        [Required]
        public Guid ModuleId { get; set; }
    }
}
