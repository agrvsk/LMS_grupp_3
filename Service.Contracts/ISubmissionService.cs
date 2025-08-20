using Domain.Models.Entities;

namespace Service.Contracts;

public interface ISubmissionService
{
    Task<List<Submission>> GetAllSubmissionsAsync();
    Task<Submission?> GetSubmissionByIdAsync(Guid submissionId);
    Task<List<Submission>> GetSubmissionsByApplicationUserIdAsync(Guid userId);
    Task<List<Submission>> GetSubmissionsByDocumentIdAsync(Guid documentId);
}