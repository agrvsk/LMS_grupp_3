using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Shared.DTOs.EntityDto
{
    public class DocumentMetadataDto
    {
        public string TempId { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? UploaderId { get; set; } 
    }
}
