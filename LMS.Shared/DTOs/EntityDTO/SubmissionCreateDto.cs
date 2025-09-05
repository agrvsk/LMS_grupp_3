using Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Shared.DTOs.EntityDto;

public record SubmissionCreateDto
{
    public string? Description { get; set; }
    public List<string> SubmitterIds { get; set; }
    public Guid AssignmentId { get; set; }
}
