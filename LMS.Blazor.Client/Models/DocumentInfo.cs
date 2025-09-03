using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;

namespace LMS.Blazor.Client.Models
{
    public record DocumentInfo
    {
        [Required(ErrorMessage = "Document name is required")]
        [StringLength(100, ErrorMessage = "Name is too long (max 100 characters)")]
        public string? Name  { get; set; }
        public IBrowserFile? File { get; set; }

        [StringLength(500, ErrorMessage = "Description is too long (max 500 characters)")]
        public string? Description { get; set; }
    }
}
