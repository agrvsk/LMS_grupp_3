using Domain.Models.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Shared.DTOs.EntityDto
{
    public class DocumentCreateDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }

        public string UploaderId { get; set; }
        public string ParentType { get; set; }
        public Guid ParentId { get; set; }

        public IFormFile File { get; set; } 
    }
}
