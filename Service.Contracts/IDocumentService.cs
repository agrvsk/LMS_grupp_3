using Domain.Models.Entities;
using LMS.Shared.DTOs.EntityDto;

namespace Service.Contracts;

public interface IDocumentService
{
    Task<List<Document>> GetAllDocumentsAsync();
    Task<Document?> GetDocumentByIdAsync(string documentId);
    Task<List<Document>> GetDocumentsByParentAsync(string parentId, string parentType);
    Task<Document> CreateDocumentAsync(DocumentCreateDto document);
    Task<Document> UpdateDocumentAsync(DocumentDto document);
    Task<bool> DeleteDocumentAsync(string documentId);
}