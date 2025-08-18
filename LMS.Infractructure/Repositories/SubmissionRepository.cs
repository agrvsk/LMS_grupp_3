using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts.Repositories;
using Domain.Models.Entities;
using LMS.Infractructure.Data;

namespace LMS.Infractructure.Repositories;

public class SubmissionRepository : RepositoryBase<Submission>, ISubmissionRepository
{
    public SubmissionRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<List<Submission>> GetAllSubmissions()
    {
        
        return await FindAllAsync(trackChanges: false).ContinueWith(task => task.Result.ToList());
    }

    public async Task<Submission?> GetSubmissionById(string submissionId)
    {
        
        return await FindByConditionAsync(s => s.Id == submissionId, trackChanges: false)
            .ContinueWith(task => task.Result.SingleOrDefault());
    }

    public Task<List<Submission>> GetSubmissionsByApplicationUserId(string userId)
    {
        
        return FindByConditionAsync(s => s.ApplicationUserId == userId, trackChanges: false)
            .ContinueWith(task => task.Result.ToList());
    }

    public Task<List<Submission>> GetSubmissionsByDocumentId(string documentId)
    {
        
        return FindByConditionAsync(s => s.DocumentId == documentId, trackChanges: false)
            .ContinueWith(task => task.Result.ToList());
    }
}
