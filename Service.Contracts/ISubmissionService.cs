using Domain.Models.Entities;
using LMS.Shared.DTOs.EntityDto;
using Microsoft.AspNetCore.Http;

namespace Service.Contracts;

public interface ISubmissionService
{
    Task<List<Submission>> GetAllSubmissionsAsync();
    Task<SubmissionDto?> GetSubmissionByIdAsync(Guid submissionId);
    Task<List<SubmissionDto>> GetSubmissionsByApplicationUserIdAsync(string userId);
    Task<List<Submission>> GetSubmissionsByDocumentIdAsync(Guid documentId);
    Task<SubmissionDto> CreateSubmissionAsync(SubmissionCreateDto submissionCreateDto, IFormFile file);
}