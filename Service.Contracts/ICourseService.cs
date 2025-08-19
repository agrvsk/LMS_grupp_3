using Domain.Models.Entities;

namespace Service.Contracts;

public interface ICourseService
{
    Task<List<Course>> GetAllCourses();
    Task<Course?> GetCourseById(string courseId);
}