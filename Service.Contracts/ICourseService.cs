using Domain.Models.Entities;
using LMS.Shared.DTOs.EntityDto;

namespace Service.Contracts;

public interface ICourseService
{
    Task<List<Course>> GetAllCoursesAsync();
    Task<Course?> GetCourseByIdAsync(Guid courseId);
    Task<Course> CreateCourseAsync(CourseCreateDto course);
    Task<Course> UpdateCourseAsync(CourseDto course);
    Task<bool> DeleteCourseAsync(Guid courseId);
}