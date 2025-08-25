using LMS.Shared.DTOs.EntityDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts
{
    public interface IActivityTypeService
    {
        Task<List<ActivityTypeDto>> GetAllActivityTypesAsync();
    }
}
