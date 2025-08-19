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

    public async Task<Document?> GetDocumentById(string documentId)
    {
        return (await uow.DocumentRepository.GetDocumentById(documentId));
    }
    public async Task<List<Document>> GetAllDocuments()
    {
        return (await uow.DocumentRepository.GetAllDocuments());
    }
    public async Task<List<Document>> GetDocumentsByParent(string parentId, string parentType)
    {
        return (await uow.DocumentRepository.GetDocumentsByParent(parentId, parentType));
    }

}
