using Domain.Models.Entities;
using LMS.Shared.DTOs.EntityDto;

namespace Service.Contracts;

public interface IDocumentService
{
    Task<List<Document>> GetAllDocumentsAsync();
    Task<Document?> GetDocumentByIdAsync(Guid documentId);
    Task<List<Document>> GetDocumentsByParentAsync(Guid parentId, string parentType);
    Task<Document> CreateDocumentAsync(DocumentCreateDto document);
    Task<Document> UpdateDocumentAsync(DocumentDto document);
    Task<bool> DeleteDocumentAsync(Guid documentId);
}