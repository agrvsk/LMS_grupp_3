using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Entities;

public class ModuleActivity
{
    public string Id { get; set; }
    [MaxLength(30)]
    [Required]
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public ActivityType Type { get; set; }
    public string ModuleId { get; set; }

}

public enum ActivityType
{
    Lecture,
    Assignment,
    Quiz,
    Project,
    Discussion
}
