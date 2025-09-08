using Domain.Models.Entities;

namespace Domain.Contracts.Repositories
{
    public interface IAssignmentRepository:IRepositoryBase<Assignment>
    {
        Task<Assignment?> GetAssignmentByIdAsync(Guid assignmentId);
        //Task<List<Assignment>> GetAllAssignmentsAsync();
        //Task<List<Assignment>> GetAssignmentsByModuleActivityIdAsync(Guid moduleActivityId);
    }
}