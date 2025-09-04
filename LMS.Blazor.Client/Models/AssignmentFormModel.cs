using System.ComponentModel.DataAnnotations;

namespace LMS.Blazor.Client.Models
{
    public class AssignmentFormModel
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        [Required]
        public DateTime DueDate { get; set; } = DateTime.Now;
        public List<DocumentInfo> Documents { get; set; } = new();
    }
}
