using Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Shared.DTOs.EntityDto
{
    public record DocumentDto
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime UploadDate { get; set; }
        public string FilePath { get; set; }
        public string UploaderId { get; set; }
        public string UploaderName { get; set; }
    }
}
