using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Shared.DTOs.EntityDto
{
    public record AssignmentCreateDto
    {
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        [Required]
        public DateTime DueDate { get; set; } = DateTime.Now;
        private string _timeString = string.Empty;
        public string TimeString
        {
            get => _timeString;
            set
            {
                _timeString = value;

                if (TimeSpan.TryParse(value, out var time))
                {
                    DueDate = new DateTime(
                        DueDate.Year,
                        DueDate.Month,
                        DueDate.Day,
                        time.Hours,
                        time.Minutes,
                        59, //so due date is inclusive of the minute whether you want it or not
                        DueDate.Kind 
                    );
                }
            }
        }
        public List<DocumentMetadataDto>? Documents { get; set; }
    }
}
