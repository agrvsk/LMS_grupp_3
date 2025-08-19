using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Entities;

public class Course
{
    public Guid Id { get; set; }
    [MaxLength(100)]
    [Required]
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public List<ApplicationUser> Students { get; set; } = new List<ApplicationUser>();
    public List<Module> Modules { get; set; }

}
