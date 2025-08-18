using Domain.Models.Entities;

namespace Domain.Contracts.Repositories;


public interface IDocumentRepository: IRepositoryBase<Document>
{
    Task<Document?> GetDocumentById(string documentId);
    Task<List<Document>> GetAllDocuments();
    Task<List<Document>> GetDocumentsByParent(string parentId, string parentType);
}