using Domain.Models.Entities;

namespace Service.Contracts;

public interface ISubmissionService
{
    Task<List<Submission>> GetAllSubmissions();
    Task<Submission?> GetSubmissionById(string submissionId);
    Task<List<Submission>> GetSubmissionsByApplicationUserId(string userId);
    Task<List<Submission>> GetSubmissionsByDocumentId(string documentId);
}