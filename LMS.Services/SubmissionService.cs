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


    public async Task<List<Submission>> GetAllSubmissions()
    {
        return await uow.SubmissionRepository.GetAllSubmissions();
    }

    public async Task<Submission?> GetSubmissionById(string submissionId)
    {

        return await uow.SubmissionRepository.GetSubmissionById(submissionId);
    }

    public Task<List<Submission>> GetSubmissionsByApplicationUserId(string userId)
    {

        return uow.SubmissionRepository.GetSubmissionsByApplicationUserId(userId);
    }

    public Task<List<Submission>> GetSubmissionsByDocumentId(string documentId)
    {

        return GetSubmissionsByDocumentId(documentId);
    }

}
