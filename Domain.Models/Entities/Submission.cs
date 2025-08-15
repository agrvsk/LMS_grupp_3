using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Entities;

public class Submission
{
    string Id { get; set; }
    string ApplicationUserId { get; set; } 
    string DocumentId { get; set; } 
}
