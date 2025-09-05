using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Contracts.Repositories;
using Domain.Models.Entities;
using LMS.Shared.DTOs.EntityDto;
using Microsoft.AspNetCore.Http;
using Service.Contracts;

namespace LMS.Services;

public class SubmissionService : ISubmissionService
{
    private readonly IUnitOfWork uow;
    private readonly IMapper mapper;
    private readonly IFileHandlerService fileHandlerService;

    public SubmissionService(IUnitOfWork uow, IMapper mapper, IFileHandlerService fileHandlerService)
    {
        this.uow = uow;
        this.mapper = mapper;
        this.fileHandlerService = fileHandlerService;
    }


    public async Task<List<Submission>> GetAllSubmissionsAsync()
    {
        return await uow.SubmissionRepository.GetAllSubmissionsAsync();
    }

    public async Task<Submission?> GetSubmissionByIdAsync(Guid submissionId)
    {

        return await uow.SubmissionRepository.GetSubmissionByIdAsync(submissionId);
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

    public async Task<SubmissionDto> CreateSubmissionAsync(SubmissionCreateDto submissionCreateDto, IFormFile file)
    {
        var assignment = await uow.AssignmentRepository.GetAssignmentByIdAsync(submissionCreateDto.AssignmentId);
        if (assignment == null)
            throw new Exception("Assignment not found.");

        var fileName = $"{Guid.NewGuid()}_{file.FileName}";
        var path = fileHandlerService.UploadFileAsync(file.OpenReadStream(), fileName, $"Uploads/Submissions/{assignment.Name}");

        var submission = mapper.Map<Submission>(submissionCreateDto);
        submission.Id = Guid.NewGuid();
        submission.SubmissionDate = DateTime.UtcNow;

        foreach (var userId in submissionCreateDto.SubmitterIds)
        {
            var user = await uow.ApplicationUserRepository.GetUserByIdAsync(userId);
            if (user == null)
                throw new Exception($"User with ID {userId} not found.");
            submission.Submitters.Add(user);
        }

        var document = new Document
        {
            Id = Guid.NewGuid(),
            Name = file.FileName,
            FilePath = path.Result,
            UploaderId = submissionCreateDto.SubmitterIds.First(),
            ParentId = submission.Id,
            ParentType = "Submission",
            UploadDate = DateTime.UtcNow
        };
        submission.DocumentId = document.Id;

        uow.DocumentRepository.Create(document);
        uow.SubmissionRepository.Create(submission);

        await uow.CompleteAsync();


        return mapper.Map<SubmissionDto>(submission);
    }

}
