using Domain.Models.Entities;
using LMS.Shared.DTOs.EntityDto;

namespace Service.Contracts;

public interface IDocumentService
{
    Task<List<DocumentDto>> GetAllDocumentsAsync();
    Task<DocumentDto?> GetDocumentByIdAsync(Guid documentId);
    Task<List<DocumentDto>> GetDocumentsByParentAsync(Guid parentId, string parentType);
    Task<DocumentDto> CreateDocumentAsync(DocumentCreateDto document, Stream fileStream);
    Task<Document> UpdateDocumentAsync(DocumentDto document);
    Task<bool> DeleteDocumentAsync(Guid documentId);
}