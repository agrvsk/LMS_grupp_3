using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Entities;

public class Course
{
    string Id { get; set; }
    [MaxLength(30)]
    [Required]
    string Name { get; set; }
    string? Description { get; set; }
    DateTime StartDate { get; set; }
    DateTime EndDate { get; set; }
    List<ApplicationUser> Students { get; set; } = new List<ApplicationUser>();
    List<Module> Modules { get; set; } = new List<Module>();
    List<Document> Documents { get; set; } = new List<Document>();
}
