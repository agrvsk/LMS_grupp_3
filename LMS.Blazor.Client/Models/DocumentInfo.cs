using Microsoft.AspNetCore.Components.Forms;

namespace LMS.Blazor.Client.Models
{
    public record DocumentInfo
    {
        public string TempId { get; set; } = Guid.NewGuid().ToString();
        public string FileName { get; set; } = default!;
        public string ContentType { get; set; } = default!;
        public byte[] Content { get; set; } = default!;
        public string? Description { get; set; }
    }
}
