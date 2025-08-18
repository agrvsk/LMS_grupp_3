using Domain.Models.Entities;
namespace Domain.Contracts.Repositories;

public interface ICourseRepository : IRepositoryBase<Course>
{
    Task<Course?> GetCourseById(string courseId);
    Task<List<Course>> GetAllCourses();
}