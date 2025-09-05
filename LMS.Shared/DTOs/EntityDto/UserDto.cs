using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Shared.DTOs.EntityDto
{
    public record UserDto
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }

    }
}
