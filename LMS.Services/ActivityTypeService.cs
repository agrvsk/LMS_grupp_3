using AutoMapper;
using Domain.Contracts.Repositories;
using Domain.Models.Exceptions;
using LMS.Shared.DTOs.EntityDto;
using Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Services
{
    public class ActivityTypeService: IActivityTypeService
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;
        public ActivityTypeService(IUnitOfWork uow, IMapper mapper) 
        {
            this.uow = uow;
            this.mapper = mapper;
        }
        public async Task<List<ActivityTypeDto>> GetAllActivityTypesAsync()
        {
            var activityTypes = await uow.ActivityTypeRepository.GetAllActivityTypesAsync();
            var activityTypeDtos = mapper.Map<List<ActivityTypeDto>>(activityTypes);
            return activityTypeDtos;
        }
    }
}
