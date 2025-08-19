using Domain.Models.Entities;
namespace Domain.Contracts.Repositories;

public interface ICourseRepository : IRepositoryBase<Course>
{
    Task<Course?> GetCourseByIdAsync(Guid courseId);
    Task<List<Course>> GetAllCoursesAsync();
}