using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Contracts.Repositories;
using Domain.Models.Entities;
using Domain.Models.Exceptions;
using LMS.Shared.DTOs.EntityDto;
using Service.Contracts;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

    public async Task<SubmissionDto?> GetSubmissionByIdAsync(Guid submissionId)
    {
        var submission = await uow.SubmissionRepository.GetSubmissionByIdAsync(submissionId);
        if(submission==null)throw new SubmissionNotFoundException(submissionId);
        return mapper.Map<SubmissionDto>(submission);
    }

    public async Task<List<SubmissionDto>> GetSubmissionsByApplicationUserIdAsync(string userId)
    {
        var data = await uow.SubmissionRepository.GetSubmissionsByApplicationUserIdAsync(userId);
        return mapper.Map<List<SubmissionDto>>(data);
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
