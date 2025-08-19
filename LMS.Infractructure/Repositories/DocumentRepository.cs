using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts.Repositories;
using Domain.Models.Entities;
using LMS.Infractructure.Data;

namespace LMS.Infractructure.Repositories;

public class DocumentRepository: RepositoryBase<Document>, IDocumentRepository
{

    public DocumentRepository(ApplicationDbContext context) : base(context)
    {
    }
    public async Task<Document?> GetDocumentByIdAsync(string documentId)
    {
        return (await FindByConditionAsync(d => d.Id == documentId, trackChanges: false)).SingleOrDefault();
    }
    public async Task<List<Document>> GetAllDocumentsAsync()
    {
        return (await FindAllAsync(trackChanges: false)).ToList();
    }
    public async Task<List<Document>> GetDocumentsByParentAsync(string parentId, string parentType)
    {
        return (await FindByConditionAsync(d => d.ParentId == parentId && d.ParentType==parentType, trackChanges: false)).ToList();
    }
}
