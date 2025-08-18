using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts.Repositories;
using Domain.Models.Entities;
using LMS.Infractructure.Data;

namespace LMS.Infractructure.Repositories;

public class DocumentRopository: RepositoryBase<Document>, IDocumentRepository
{

    public DocumentRopository(ApplicationDbContext context) : base(context)
    {
    }
    public async Task<Document?> GetDocumentById(string documentId)
    {
        return (await FindByConditionAsync(d => d.Id == documentId, trackChanges: false)).SingleOrDefault();
    }
    public async Task<List<Document>> GetAllDocuments()
    {
        return (await FindAllAsync(trackChanges: false)).ToList();
    }
    public async Task<List<Document>> GetDocumentsByParent(string parentId, string parentType)
    {
        return (await FindByConditionAsync(d => d.ParentId == parentId && d.ParentType==parentType, trackChanges: false)).ToList();
    }
}
