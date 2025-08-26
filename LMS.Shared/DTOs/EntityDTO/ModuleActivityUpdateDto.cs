using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models.Entities;

namespace LMS.Shared.DTOs.EntityDto;

public record ModuleActivityUpdateDto
{
    public Guid Id { get; set; }
    [MaxLength(100)]
    [Required]
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int ActivityTypeId { get; set; }
    
//    public ActivityType Type { get; set; }
    public Guid ModuleId { get; set; }

}
