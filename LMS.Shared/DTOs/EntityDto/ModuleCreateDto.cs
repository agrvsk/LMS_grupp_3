using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Shared.DTOs.EntityDTO
{
    public record ModuleCreateDTO
    {
        public string Id { get; set; }
        [MaxLength(30)]
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public List<ModuleActivityDTO> ModuleActivities { get; set; } = new List<ModuleActivityDTO>();
    }
}
