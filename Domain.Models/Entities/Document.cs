using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Entities;

public class Document
{
    public Guid Id { get; set; }
    [MaxLength(100)]
    [Required]
    public string Name { get; set; }
    public string? Description { get; set; }
    public DateTime UploadDate { get; set; }
    public string FilePath { get; set; }
    public string ParentType { get; set; }// e.g., "Course", "Module", "Activity", "Submission"
    public Guid ParentId { get; set; }
    public string? UploaderId { get; set; }
    public ApplicationUser? Uploader { get; set; }
    public string FileType { get; set; }
}
