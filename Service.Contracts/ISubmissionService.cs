using Domain.Models.Entities;

namespace Service.Contracts;

public interface ISubmissionService
{
    Task<List<Submission>> GetAllSubmissionsAsync();
    Task<Submission?> GetSubmissionByIdAsync(string submissionId);
    Task<List<Submission>> GetSubmissionsByApplicationUserIdAsync(string userId);
    Task<List<Submission>> GetSubmissionsByDocumentIdAsync(string documentId);
}