using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts.Repositories;
using Domain.Models.Entities;
using Service.Contracts;

namespace LMS.Services;

public class DocumentService : IDocumentService
{
    private readonly IUnitOfWork uow;

    public DocumentService(IUnitOfWork uow)
    {
        this.uow = uow;
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

}
