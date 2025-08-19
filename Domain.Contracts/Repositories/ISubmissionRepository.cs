using Domain.Models.Entities;
namespace Domain.Contracts.Repositories;
public interface ISubmissionRepository: IInternalRepositoryBase<Submission>
{
    Task<Submission?> GetSubmissionByIdAsync(string submissionId);
    Task<List<Submission>> GetAllSubmissionsAsync();
    Task<List<Submission>> GetSubmissionsByDocumentIdAsync(string documentId);
    Task<List<Submission>> GetSubmissionsByApplicationUserIdAsync(string userId);
    
}
