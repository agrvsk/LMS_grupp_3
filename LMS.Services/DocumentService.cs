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

public class DocumentService : IDocumentService
{
    private readonly IUnitOfWork uow;
    private readonly IMapper mapper;
    private readonly IFileHandlerService fileHandlerService;

    public DocumentService(IUnitOfWork uow, IMapper mapper, IFileHandlerService fileHandlerService)
    {
        this.uow = uow;
        this.mapper = mapper;
        this.fileHandlerService = fileHandlerService;
    }

    public async Task<DocumentDto?> GetDocumentByIdAsync(Guid documentId)
    {
        var document = await uow.DocumentRepository.GetDocumentByIdAsync(documentId);
        var documentDto = mapper.Map<DocumentDto>(document);
        return documentDto;
    }
    public async Task<List<DocumentDto>> GetAllDocumentsAsync()
    {
        var documents = await uow.DocumentRepository.GetAllDocumentsAsync();
        var documentDtos = mapper.Map<List<DocumentDto>>(documents);
        return documentDtos;
    }
    public async Task<List<DocumentDto>> GetDocumentsByParentAsync(Guid parentId, string parentType)
    {
        var documents = await uow.DocumentRepository.GetDocumentsByParentAsync(parentId, parentType);
        var documentDtos = mapper.Map<List<DocumentDto>>(documents);
        return documentDtos;
    }
    public async Task<DocumentDto> CreateDocumentAsync(DocumentCreateDto documentDto, Stream fileStream)
    {
        var document = mapper.Map<Document>(documentDto);
        document.Id = Guid.NewGuid();
        document.UploadDate = DateTime.UtcNow;

        var filePath = await fileHandlerService.UploadFileAsync(fileStream, $"{documentDto.Name}_{document.Id}", $"Uploads/{document.ParentType}");
        document.FilePath = filePath;

        uow.DocumentRepository.Create(document);
        await uow.CompleteAsync();
        return mapper.Map<DocumentDto>(document);
    }
    public async Task<Document?> UpdateDocumentAsync(DocumentDto documentDto)
    {
        var document = mapper.Map<Document>(documentDto);
        uow.DocumentRepository.Update(document);
        await uow.CompleteAsync();
        return document;
    }
    public async Task<bool> DeleteDocumentAsync(Guid documentId)
    {
        var document = await uow.DocumentRepository.GetDocumentByIdAsync(documentId);
        if (document != null)
        {
            uow.DocumentRepository.Delete(document);
            await uow.CompleteAsync();
            return true;
        }
        return false;
    }


}
