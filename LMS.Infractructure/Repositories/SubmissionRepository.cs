using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts.Repositories;
using Domain.Models.Entities;
using LMS.Infractructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infractructure.Repositories;

public class SubmissionRepository : RepositoryBase<Submission>, ISubmissionRepository
{
    private ApplicationDbContext context;

    public SubmissionRepository(ApplicationDbContext context) : base(context)
    {
        this.context = context;
    }

    public async Task<List<Submission>> GetAllSubmissionsAsync()
    {
        
        return (await FindAllAsync(trackChanges: false)).ToList();
    }

    public async Task<Submission?> GetSubmissionByIdAsync(Guid submissionId)
    {

        return (await FindByConditionAsync(s => s.Id == submissionId, trackChanges: false)).SingleOrDefault();
    }

    public async Task<List<Submission>> GetSubmissionsByApplicationUserIdAsync(string userId)
    {
        
        return (await FindByConditionAsync(s => s.Submitters.Any(u => u.Id == userId), trackChanges: false)).ToList();
    }

    public async Task<List<Submission>> GetSubmissionsByDocumentIdAsync(Guid documentId)
    {
        
        return (await FindByConditionAsync(s => s.DocumentId == documentId, trackChanges: false)).ToList();
    }

    public async Task<List<Submission>> GetSubmissionsByAssignmentIdAsync(Guid assignmentId)
    {
        return (await FindByConditionAsync(s => s.AssignmentId == assignmentId, trackChanges: false)).ToList();
    }


    public void CreateModule(Submission submission)
    {
        context.Submissions.Add(submission);
    }
    public void UpdateModule(Submission submission)
    {
        context.Submissions.Update(submission);
    }
    public void DeleteModule(Submission submission)
    {
        context.Submissions.Remove(submission);
    }

}
