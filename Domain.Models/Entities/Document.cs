using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Entities;

public class Document
{
    public string Id { get; set; }
    [MaxLength(30)]
    [Required]
    public string Name { get; set; }
    public string? Description { get; set; }
    public DateTime UploadDate { get; set; }
    public string FilePath { get; set; }
    public string ParentType { get; set; }// e.g., "Course", "Module", "Activity", "CourseSubmission", "ModuleSubmission", "ActivitySubmission"
    public string ParentId { get; set; }

    public ApplicationUser Uploader { get; set; }
}
