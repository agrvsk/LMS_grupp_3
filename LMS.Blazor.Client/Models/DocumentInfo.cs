using Microsoft.AspNetCore.Components.Forms;

namespace LMS.Blazor.Client.Models
{
    public record DocumentInfo
    {
        public IBrowserFile? File { get; set; }
        public string? Description { get; set; }
    }
}
