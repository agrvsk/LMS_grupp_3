using Domain.Models.Entities;
using LMS.Shared.DTOs.EntityDto;

namespace Service.Contracts;

public interface ICourseService
{
    Task<List<CourseDto>> GetAllCoursesAsync();
    Task<CourseDto?> GetCourseByIdAsync(Guid courseId);
    Task<CourseDto> CreateCourseAsync(CourseCreateDto course);
    Task<CourseDto> UpdateCourseAsync(CourseUpdateDto course);
    Task<bool> DeleteCourseAsync(Guid courseId);
    Task<List<AssignmentDto>> GetAssignmentsByCourseIdAsync(Guid courseId);
}