using Domain.Models.Entities;

namespace Service.Contracts;

public interface IDocumentService
{
    Task<List<Document>> GetAllDocumentsAsync();
    Task<Document?> GetDocumentByIdAsync(string documentId);
    Task<List<Document>> GetDocumentsByParentAsync(string parentId, string parentType);
}