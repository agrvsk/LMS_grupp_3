using Domain.Models.Entities;

namespace Domain.Contracts.Repositories;


public interface IDocumentRepository: IRepositoryBase<Document>
{
    Task<Document?> GetDocumentByIdAsync(Guid documentId);
    Task<List<Document>> GetAllDocumentsAsync();
    Task<List<Document>> GetDocumentsByParentAsync(Guid parentId, string parentType);
}