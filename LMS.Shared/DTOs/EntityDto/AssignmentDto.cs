using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Shared.DTOs.EntityDto
{
    public record AssignmentDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime DueDate { get; set; }
        public Guid? AttachedDocumentId { get; set; }
        public DocumentDto? AttachedDocument { get; set; }
    }
}
