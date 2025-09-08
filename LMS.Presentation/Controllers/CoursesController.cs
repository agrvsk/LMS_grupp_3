using Domain.Models.Exceptions;
using LMS.Shared.DTOs.EntityDto;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;

namespace LMS.Presentation.Controllers
{
    [Route("/courses")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public CoursesController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCourses()
        {
            var courses = await _serviceManager.CourseService.GetAllCoursesAsync();
            return Ok(courses);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCourseById(Guid id)
        {
            var course = await _serviceManager.CourseService.GetCourseByIdAsync(id);
            if (course == null)
            {
                throw new CourseNotFoundException(id);
            }
            return Ok(course);
        }
        [HttpGet("{id}/assignments")]
        public async Task<IActionResult> GetAssignmentsByCourseId(Guid id)
        {
            var assignments = await _serviceManager.CourseService.GetAssignmentsByCourseIdAsync(id);
            if (assignments == null)
            {
                return NotFound();
            }
            return Ok(assignments);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCourse([FromBody] CourseCreateDto courseDto)
        {
            if (courseDto == null)
            {
                return BadRequest("Course data is null");
            }
            if (!_serviceManager.DateValidationService.ValidateCourseDates(courseDto.StartDate, courseDto.EndDate))
            {
                ModelState.AddModelError("DateValidation", "End date must be greater than start date.");
                return BadRequest(ModelState);
            }
            try
            {
                var createdCourse = await _serviceManager.CourseService.CreateCourseAsync(courseDto);
                
                return CreatedAtAction(nameof(GetCourseById), new { id = createdCourse.Id }, createdCourse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourse(Guid id, [FromBody] CourseUpdateDto courseDto)
        {
            if (courseDto == null || id != courseDto.Id)
            {
                return BadRequest("Course data is invalid");
            }
            if (!_serviceManager.DateValidationService.ValidateCourseDates(courseDto.StartDate, courseDto.EndDate))
            {
                ModelState.AddModelError("DateValidation", "End date must be greater than start date.");
                return BadRequest(ModelState);
            }
            var updatedCourse = await _serviceManager.CourseService.UpdateCourseAsync(courseDto);
            if (updatedCourse == null)
            {
                return NotFound();
            }
            return Ok(updatedCourse);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(Guid id)
        {
            var result = await _serviceManager.CourseService.DeleteCourseAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
