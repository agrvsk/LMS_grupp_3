using Domain.Contracts.Repositories;
using Domain.Models.Entities;
using LMS.Infractructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Infractructure.Repositories
{
    public class ActivityTypeRepository: RepositoryBase<ActivityType>, IActivityTypeRepository
    {
        public ActivityTypeRepository(ApplicationDbContext context) : base(context)
        {

        }
        public async Task<List<ActivityType>> GetAllActivityTypesAsync()
        {
            return (await FindAllAsync(trackChanges: false)).ToList();
        }
    }
}
