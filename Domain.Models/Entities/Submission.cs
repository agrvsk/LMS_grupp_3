using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Entities;

public class Submission
{
    public Guid Id { get; set; }
    public DateTime SubmissionDate { get; set; }
    public Guid ApplicationUserId { get; set; } 
    public Guid DocumentId { get; set; } 
}
