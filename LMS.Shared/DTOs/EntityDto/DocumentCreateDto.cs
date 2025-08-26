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
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        public string FilePath { get; set; }

        public UserDto Uploader { get; set; }
        public string ParentType { get; set; }
        public Guid ParentId { get; set; }
    }
}
