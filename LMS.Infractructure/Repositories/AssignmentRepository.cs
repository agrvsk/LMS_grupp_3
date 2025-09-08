using Domain.Contracts.Repositories;
using Domain.Models.Entities;
using LMS.Infractructure.Data;
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
        public AssignmentRepository(ApplicationDbContext context) : base(context)
        {
            
        }
        public async Task<Assignment?> GetAssignmentByIdAsync(Guid assignmentId)
        {
            return (await FindByConditionAsync(a => a.Id == assignmentId, trackChanges: false)).SingleOrDefault();
        }
    }
}
