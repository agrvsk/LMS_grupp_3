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
    public List<ApplicationUser> Submitters { get; set; } 
    public Guid DocumentId { get; set; } 
    public Document? SubmissionDocument { get; set; }
    public Guid AssignmentId { get; set; }
    public Assignment Assignment { get; set; }
}
