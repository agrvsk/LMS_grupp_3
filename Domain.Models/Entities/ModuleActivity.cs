using System.ComponentModel.DataAnnotations;

namespace Domain.Models.Entities;

public class ModuleActivity
{
    public Guid Id { get; set; }
    [MaxLength(100)]
    [Required]
    public string Name { get; set; }
    public string? Description { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public int ActivityTypeId { get; set; }
    public ActivityType Type { get; set; }

    public Guid ModuleId { get; set; }
    public Module Module { get; set; }
    public List<Assignment>? Assignments { get; set; }

}


