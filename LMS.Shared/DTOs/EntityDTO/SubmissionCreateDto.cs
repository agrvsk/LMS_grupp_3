using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Shared.DTOs.EntityDto;

public record SubmissionCreateDto
{
    public DateTime SubmissionDate { get; set; }
    public string ApplicationUserId { get; set; }
    public Guid DocumentId { get; set; }

}
