using Domain.Models.Entities;
namespace Domain.Contracts.Repositories;
public interface ISubmissionRepository: IInternalRepositoryBase<Submission>
{
    Task<Submission?> GetSubmissionById(string submissionId);
    Task<List<Submission>> GetAllSubmissions();
    Task<List<Submission>> GetSubmissionsByDocumentId(string documentId);
    Task<List<Submission>> GetSubmissionsByApplicationUserId(string userId);
    
}
