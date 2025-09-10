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
using LMS.Shared.DTOs.EntityDTO;
using Microsoft.AspNetCore.Identity;
using Service.Contracts;

namespace LMS.Services;

public class DocumentService : IDocumentService
{
    private readonly IUnitOfWork uow;
    private readonly IMapper mapper;
    private readonly IFileHandlerService fileHandlerService;
    private readonly UserManager<ApplicationUser> userManager;

    public DocumentService(IUnitOfWork uow, IMapper mapper, IFileHandlerService fileHandlerService, UserManager<ApplicationUser> userManager)
    {
        this.uow = uow;
        this.mapper = mapper;
        this.fileHandlerService = fileHandlerService;
        this.userManager = userManager;
    }

    public async Task<DocumentDto?> GetDocumentByIdAsync(Guid documentId)
    {
        var document = await uow.DocumentRepository.GetDocumentByIdAsync(documentId);
        if (document == null) throw new DocumentNotFoundException(documentId);
        var documentDto = mapper.Map<DocumentDto>(document);
        return documentDto;
    }
    public async Task<List<DocumentDto>> GetAllDocumentsAsync()
    {
        var documents = await uow.DocumentRepository.GetAllDocumentsAsync();
        var documentDtos = mapper.Map<List<DocumentDto>>(documents);
        foreach (var doc in documentDtos)
        {
            var user = await userManager.FindByIdAsync(doc.UploaderId);
            doc.UploaderName = await userManager.GetEmailAsync(user);
        }
        return documentDtos.OrderBy(c => c.UploadDate).ToList();
    }
    public async Task<List<DocumentDto>> GetDocumentsByParentAsync(Guid parentId, string parentType)
    {
        var documents = await uow.DocumentRepository.GetDocumentsByParentAsync(parentId, parentType);
        var documentDtos = mapper.Map<List<DocumentDto>>(documents);
        foreach (var doc in documentDtos)
        {
            var userEmail = (await userManager.FindByIdAsync(doc.UploaderId))?.Email;
            doc.UploaderName = userEmail ?? "Unknown";
        }
        return documentDtos;
    }
    public async Task<DocumentDto> CreateDocumentAsync(DocumentCreateDto documentDto, Stream fileStream)
    {
        var document = mapper.Map<Document>(documentDto);
        document.Id = Guid.NewGuid();
        document.UploadDate = DateTime.UtcNow;

        var filePath = await fileHandlerService.UploadFileAsync(fileStream, $"{documentDto.Name}_{document.Id}{documentDto.FileType}", $"Uploads/{document.ParentType}");
        document.FilePath = filePath;

        uow.DocumentRepository.Create(document);

        if (documentDto.Submissions != null && documentDto.Submissions.Count > 0)
        {
            foreach (var submissionDto in documentDto.Submissions)
            {
                var sub = mapper.Map<Submission>(submissionDto);
                sub.DocumentId = document.Id;
                uow.SubmissionRepository.Create(sub);
            }
        }
        await uow.CompleteAsync();
        return mapper.Map<DocumentDto>(document);
    }
    public async Task<Document?> UpdateDocumentAsync(DocumentEditDto documentDto)
    {
        var document = await uow.DocumentRepository.GetDocumentByIdAsync(documentDto.Id);
        if (document == null)
            return null;

        // Uppdatera bara de fält som får ändras
        document.Name = documentDto.Name;
        document.Description = documentDto.Description;

        uow.DocumentRepository.Update(document);
        await uow.CompleteAsync();

        return document;


        //var document = mapper.Map<Document>(documentDto);
        //uow.DocumentRepository.Update(document);
        //await uow.CompleteAsync();
        //return document;
    }
    public async Task<bool> DeleteDocumentAsync(Guid documentId)
    {
        var document = await uow.DocumentRepository.GetDocumentByIdAsync(documentId);
        if (document != null)
        {
            await fileHandlerService.DeleteFileAsync(document.FilePath);
            if (document.ParentType=="submission")
            {
                var submissions = await uow.SubmissionRepository.GetSubmissionsByDocumentIdAsync(document.Id);
                foreach (var sub in submissions)
                {
                    uow.SubmissionRepository.Delete(sub);
                }
            }
            uow.DocumentRepository.Delete(document);
            await uow.CompleteAsync();
            return true;
        }
        return false;
    }
    public async Task<bool> DeleteUserDocumentsAsync(string userId)
    {
        try
        {
            var documents = await uow.DocumentRepository.GetDocumentsByUploaderIdAsync(userId);
            if (documents.Any())
            {
                foreach (var document in documents)
                {
                    await fileHandlerService.DeleteFileAsync(document.FilePath);
                    if (document.ParentType=="submission")
                        {
                        var submissions = await uow.SubmissionRepository.GetSubmissionsByDocumentIdAsync(document.Id);
                        foreach (var sub in submissions)
                        {
                            uow.SubmissionRepository.Delete(sub);
                        }
                    }
                    
                    uow.DocumentRepository.Delete(document);
                }
                await uow.CompleteAsync();
            }
                return true;
            
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while deleting user documents: {ex.Message}");
            return false;
        }
    }
    public async Task<(Stream Stream, string ContentType, string FileName)?> DownloadDocumentAsync(Guid documentId)
    {
        var document = await uow.DocumentRepository.GetDocumentByIdAsync(documentId);
        if (document == null)
            return null;

        return await fileHandlerService.GetFileByPathAsync(document.FilePath);
    }
}
