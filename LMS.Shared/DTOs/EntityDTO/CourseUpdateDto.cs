using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Shared.DTOs.EntityDto;

public record CourseUpdateDto
{
    public Guid Id { get; set; }
    [MaxLength(30)]
    [Required]
    public string Name { get; set; }
    [Required]
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

}
