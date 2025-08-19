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
        public async Task<IActionResult> GetCourseById(string id)
        {
            var course = await _serviceManager.CourseService.GetCourseByIdAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            return Ok(course);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCourse([FromBody] CourseDto courseDto)
        {
            if (courseDto == null)
            {
                return BadRequest("Course data is null");
            }
            var createdCourse = await _serviceManager.CourseService.CreateCourseAsync(courseDto);
            return CreatedAtAction(nameof(GetCourseById), new { id = createdCourse.Id }, createdCourse);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourse(int id, [FromBody] CourseDto courseDto)
        {
            if (courseDto == null || id != courseDto.Id)
            {
                return BadRequest("Course data is invalid");
            }
            var updatedCourse = await _serviceManager.CourseService.UpdateCourseAsync(courseDto);
            if (updatedCourse == null)
            {
                return NotFound();
            }
            return Ok(updatedCourse);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
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
