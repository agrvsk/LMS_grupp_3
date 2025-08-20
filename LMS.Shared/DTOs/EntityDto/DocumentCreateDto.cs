using Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Shared.DTOs.EntityDto
{
    public record DocumentCreateDto
    {
        [MaxLength(30)]
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime UploadDate { get; set; }
        public string FilePath { get; set; }

        public ApplicationUser Uploader { get; set; }
        public string ParentType { get; set; }// e.g., "Course", "Module", "Activity", "CourseSubmission", "ModuleSubmission", "ActivitySubmission"
        public string ParentId { get; set; }
    }
}
