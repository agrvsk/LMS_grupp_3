using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Contracts.Repositories;
using Domain.Models.Entities;
using LMS.Shared.DTOs.EntityDto;
using Service.Contracts;

namespace LMS.Services;

public class SubmissionService : ISubmissionService
{
    private readonly IUnitOfWork uow;
    private readonly IMapper mapper;

    public SubmissionService(IUnitOfWork uow, IMapper mapper)
    {
        this.uow = uow;
        this.mapper = mapper;
    }


    public async Task<List<Submission>> GetAllSubmissionsAsync()
    {
        return await uow.SubmissionRepository.GetAllSubmissionsAsync();
    }

    public async Task<Submission?> GetSubmissionByIdAsync(Guid submissionId)
    {

        return await uow.SubmissionRepository.GetSubmissionByIdAsync(submissionId);
    }

    public async Task<List<Submission>> GetSubmissionsByApplicationUserIdAsync(string userId)
    {

        return await uow.SubmissionRepository.GetSubmissionsByApplicationUserIdAsync(userId);
    }

    public async Task<List<Submission>> GetSubmissionsByDocumentIdAsync(Guid documentId)
    {

        return await uow.SubmissionRepository.GetSubmissionsByDocumentIdAsync(documentId);
    }

    public async Task<SubmissionDto> CreateSubmissionAsync(SubmissionCreateDto submissionCreateDto)
    {
        var submission = mapper.Map<Submission>(submissionCreateDto);
        uow.SubmissionRepository.Create(submission);
        await uow.CompleteAsync();
        var submissionDto = mapper.Map<SubmissionDto>(submission);
        return submissionDto;
    }

}
