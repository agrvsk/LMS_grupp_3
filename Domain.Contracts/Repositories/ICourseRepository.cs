using Domain.Models.Entities;
namespace Domain.Contracts.Repositories;

public interface ICourseRepository : IRepositoryBase<Course>
{
    Task<Course?> GetCourseByIdAsync(string courseId);
    Task<List<Course>> GetAllCoursesAsync();
    void CreateCourse(Course course);
    void UpdateCourse(Course course);
    void DeleteCourse(Course course);
}