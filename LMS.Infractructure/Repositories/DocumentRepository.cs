using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts.Repositories;
using Domain.Models.Entities;
using LMS.Infractructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infractructure.Repositories;

public class DocumentRepository: RepositoryBase<Document>, IDocumentRepository
{
    private readonly ApplicationDbContext _context;
    public DocumentRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }
    public async Task<Document?> GetDocumentByIdAsync(Guid documentId)
    {
        return (await FindByConditionAsync(d => d.Id == documentId, trackChanges: false)).SingleOrDefault();
    }
    public async Task<List<Document>> GetAllDocumentsAsync()
    {
        return _context.Documents.AsQueryable().Include(d => d.Uploader).ToList();
    }
    public async Task<List<Document>> GetDocumentsByParentAsync(Guid parentId, string parentType)
    {
        return (await FindByConditionAsync(d => d.ParentId == parentId && d.ParentType.ToString().ToLower() == parentType.ToString().ToLower(), trackChanges: false)).ToList();
    }
    public async Task<List<Document>> GetDocumentsByUploaderIdAsync(string userId)
    {
        return (await FindByConditionAsync(d => d.UploaderId == userId, trackChanges: false)).ToList();
    }
}
