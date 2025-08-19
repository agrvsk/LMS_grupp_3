using Domain.Models.Entities;

namespace Service.Contracts;

public interface ICourseService
{
    Task<List<Course>> GetAllCoursesAsync();
    Task<Course?> GetCourseByIdAsync(string courseId);
}