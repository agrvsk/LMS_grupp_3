using System.ComponentModel.DataAnnotations;

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


