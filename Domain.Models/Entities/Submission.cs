using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Entities;

public class Submission
{
    public string Id { get; set; }
    public DateTime SubmissionDate { get; set; }
    public string ApplicationUserId { get; set; } 
    public string DocumentId { get; set; } 
}
