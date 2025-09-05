using AutoMapper;
using Domain.Contracts.Repositories;
using LMS.Shared.DTOs.EntityDto;
using Service.Contracts;

namespace LMS.Services
{
    public class AssignmentService : IAssignmentService
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;
        public AssignmentService(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }
        public async Task<AssignmentDto> GetAssignmentById(Guid id)
        {
            var assignment = await uow.AssignmentRepository.GetAssignmentByIdAsync(id);
            if (assignment == null)
                throw new Exception("Assignment not found.");
            return mapper.Map<AssignmentDto>(assignment);
        }
    }
}
