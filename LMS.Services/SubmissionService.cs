using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts.Repositories;
using Domain.Models.Entities;
using Service.Contracts;

namespace LMS.Services;

public class SubmissionService : ISubmissionService
{
    private readonly IUnitOfWork uow;

    public SubmissionService(IUnitOfWork uow)
    {
        this.uow = uow;
    }


    public async Task<List<Submission>> GetAllSubmissionsAsync()
    {
        return await uow.SubmissionRepository.GetAllSubmissionsAsync();
    }

    public async Task<Submission?> GetSubmissionByIdAsync(Guid submissionId)
    {

        return await uow.SubmissionRepository.GetSubmissionByIdAsync(submissionId);
    }

    public async Task<List<Submission>> GetSubmissionsByApplicationUserIdAsync(Guid userId)
    {

        return await uow.SubmissionRepository.GetSubmissionsByApplicationUserIdAsync(userId);
    }

    public async Task<List<Submission>> GetSubmissionsByDocumentIdAsync(Guid documentId)
    {

        return await uow.SubmissionRepository.GetSubmissionsByDocumentIdAsync(documentId);
    }

}
