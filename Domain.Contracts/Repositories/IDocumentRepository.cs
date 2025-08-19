using Domain.Models.Entities;

namespace Domain.Contracts.Repositories;


public interface IDocumentRepository: IRepositoryBase<Document>
{
    Task<Document?> GetDocumentByIdAsync(string documentId);
    Task<List<Document>> GetAllDocumentsAsync();
    Task<List<Document>> GetDocumentsByParentAsync(string parentId, string parentType);
}