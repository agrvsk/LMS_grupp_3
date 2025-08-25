using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Shared.DTOs.EntityDto
{
    public record ActivityTypeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
