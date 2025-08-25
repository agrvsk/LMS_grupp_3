using Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts.Repositories
{
    public interface IActivityTypeRepository: IRepositoryBase<ActivityType>
    {
        public Task<List<ActivityType>> GetAllActivityTypesAsync();
    }
}
