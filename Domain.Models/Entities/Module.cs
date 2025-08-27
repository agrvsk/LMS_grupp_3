using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Entities;

public class Module
{
    public Guid Id { get; set; }
    [MaxLength(100)]
    [Required]
    public string Name { get; set; }
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public Guid CourseId { get; set; }

    public List<ModuleActivity> ModuleActivities { get; set; } = new List<ModuleActivity>();
}
