using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Entities;

public class Document
{
    string Id { get; set; }
    [MaxLength(30)]
    [Required]
    string Name { get; set; }
    string? Description { get; set; }
    DateTime UploadDate { get; set; }
    string FilePath { get; set; }
    string ParentType { get; set; }
    string ParentId { get; set; }

    ApplicationUser Uploader { get; set; }
}
