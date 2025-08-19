using Domain.Models.Entities;

namespace Service.Contracts;

public interface IDocumentService
{
    Task<List<Document>> GetAllDocuments();
    Task<Document?> GetDocumentById(string documentId);
    Task<List<Document>> GetDocumentsByParent(string parentId, string parentType);
}