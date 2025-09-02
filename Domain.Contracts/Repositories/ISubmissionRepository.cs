using Domain.Models.Entities;
namespace Domain.Contracts.Repositories;
public interface ISubmissionRepository: IInternalRepositoryBase<Submission>
{
    Task<Submission?> GetSubmissionByIdAsync(Guid submissionId);
    Task<List<Submission>> GetAllSubmissionsAsync();
    Task<List<Submission>> GetSubmissionsByDocumentIdAsync(Guid documentId);
    Task<List<Submission>> GetSubmissionsByApplicationUserIdAsync(string userId);

    void Create(Submission submission);
    void Update(Submission submission);
    void Delete(Submission submission);

}
