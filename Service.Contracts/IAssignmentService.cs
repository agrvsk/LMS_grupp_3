using LMS.Shared.DTOs.EntityDto;

namespace Service.Contracts
{
    public interface IAssignmentService
    {
        Task<AssignmentDto> GetAssignmentById(Guid id);
    }
}