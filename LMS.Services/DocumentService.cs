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

    public DocumentService(IUnitOfWork uow, IMapper mapper)
    {
        this.uow = uow;
        this.mapper = mapper;
    }

    public async Task<Document?> GetDocumentByIdAsync(string documentId)
    {
        return (await uow.DocumentRepository.GetDocumentByIdAsync(documentId));
    }
    public async Task<List<Document>> GetAllDocumentsAsync()
    {
        return (await uow.DocumentRepository.GetAllDocumentsAsync());
    }
    public async Task<List<Document>> GetDocumentsByParentAsync(string parentId, string parentType)
    {
        return (await uow.DocumentRepository.GetDocumentsByParentAsync(parentId, parentType));
    }
    public async Task<Document?> CreateDocumentAsync(DocumentCreateDto documentDto)
    {
        var document = mapper.Map<Document>(documentDto);
        uow.DocumentRepository.Create(document);
        await uow.CompleteAsync();
        return document;
    }
    public async Task<Document?> UpdateDocumentAsync(DocumentDto documentDto)
    {
        var document = mapper.Map<Document>(documentDto);
        uow.DocumentRepository.Update(document);
        await uow.CompleteAsync();
        return document;
    }
    public async Task<bool> DeleteDocumentAsync(string documentId)
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
