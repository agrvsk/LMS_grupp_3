using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;

namespace LMS.Blazor.Client.Models
{
    public record DocumentInfo
    {
        public string TempId { get; set; } = Guid.NewGuid().ToString();
        [Required(ErrorMessage = "Document name is required")]
        [StringLength(100, ErrorMessage = "Name is too long (max 100 characters)")]
        public string FileName { get; set; } = default!;
        public string ContentType { get; set; } = default!;
        public byte[] Content { get; set; } = default!;
        public string? Description { get; set; }
    }
}
