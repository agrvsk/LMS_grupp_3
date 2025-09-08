using Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Shared.DTOs.EntityDto;

public record SubmissionDto
{
    public Guid Id { get; set; }
    public Guid AssignmentId { get; set; }
    public List<UserDto> Submitters { get; set; } = new();
    public DocumentDto Document { get; set; } = default!;
    public DateTime SubmissionDate { get; set; }
    public string? Description { get; set; }

    public DocumentDto? DocumentDto { get; set; }
}
