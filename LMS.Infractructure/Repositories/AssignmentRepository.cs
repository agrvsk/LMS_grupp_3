using Domain.Contracts.Repositories;
using Domain.Models.Entities;
using LMS.Infractructure.Data;
using LMS.Shared.DTOs.EntityDto;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Infractructure.Repositories
{
    public class AssignmentRepository : RepositoryBase<Assignment>, IAssignmentRepository
    {
        private readonly ApplicationDbContext _context;
        public AssignmentRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<Assignment?> GetAssignmentByIdAsync(Guid assignmentId)
        {
            return (await FindByConditionAsync(a => a.Id == assignmentId, trackChanges: false)).SingleOrDefault();
        }
        public async Task<List<Assignment>?> GetAssignmentsByActivityIdAsync(Guid activityId)
        {
            List<Assignment>? assignments = new();
            List<Document> docs = _context.Documents.Where(d => d.ParentId == activityId && d.ParentType == "Activity").ToList();
            if (docs == null || docs.Count==0) return null;
            foreach (var doc in docs)
            {
                var asss = _context.Assignments.Where(a => a.Documents.FirstOrDefault() == doc).ToList();
                if (asss != null && asss.Count > 0)
                    assignments.AddRange(asss);
            }
            return assignments;
        }
        public async Task<List<Assignment>> GetAllAssignmentsAsync()
        {
            return (await FindAllAsync(trackChanges: false)).ToList();
        }
    }
}
