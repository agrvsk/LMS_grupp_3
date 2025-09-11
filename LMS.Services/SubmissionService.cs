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
using Microsoft.AspNetCore.Http;
using Service.Contracts;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        return (await uow.SubmissionRepository.GetAllSubmissionsAsync()).OrderBy(c => c.SubmissionDate).ToList();
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
        return mapper.Map<List<SubmissionDto>>(data).OrderBy(c => c.SubmissionDate).ToList();
    }

    public async Task<List<Submission>> GetSubmissionsByDocumentIdAsync(Guid documentId)
    {

        return (await uow.SubmissionRepository.GetSubmissionsByDocumentIdAsync(documentId)).OrderBy(c => c.SubmissionDate).ToList();
    }

    public async Task<List<SubmissionDto>> GetSubmissionsByAssignmentIdAsync(Guid assignmentId)
    {
        var data = await uow.SubmissionRepository.GetSubmissionsByAssignmentIdAsync(assignmentId);
        return mapper.Map<List<SubmissionDto>>(data).OrderBy(c => c.SubmissionDate).ToList();
    }

    public async Task<SubmissionDto> CreateSubmissionAsync(SubmissionCreateDto submissionCreateDto, IFormFile file)
    {
        var assignment = await uow.AssignmentRepository.GetAssignmentByIdAsync(submissionCreateDto.AssignmentId);
        if (assignment == null)
            throw new Exception("Assignment not found.");

        var extension = Path.GetExtension(file.FileName);
        var safeFileName = Path.GetFileNameWithoutExtension(file.FileName);
        var docId = Guid.NewGuid();
        var fileName = $"{safeFileName}_{docId}{extension}";
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
            Id = docId,
            Name = Path.GetFileNameWithoutExtension(file.FileName),
            FilePath = path.Result,
            UploaderId = submissionCreateDto.SubmitterIds.First(),
            ParentId = submission.Id,
            ParentType = "Submission",
            UploadDate = DateTime.UtcNow,
            FileType = Path.GetExtension(file.FileName)
        };
        submission.DocumentId = document.Id;

        uow.DocumentRepository.Create(document);
        uow.SubmissionRepository.Create(submission);

        await uow.CompleteAsync();


        return mapper.Map<SubmissionDto>(submission);
    }

}
