using Microsoft.AspNetCore.Identity;

namespace Domain.Models.Entities;

public class ApplicationUser : IdentityUser
{
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpireTime { get; set; }

    public Guid? CourseId { get; set; }
    public List<Submission> Submissions { get; set; } = new List<Submission>();
}
