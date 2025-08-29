using LMS.Services;
using LMS.Shared.DTOs.EntityDto;
using LMS.UnitTests.Setups;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.UnitTests.Services
{
    public class ScheduleServiceTests : ServiceTestBase
    {
        private readonly ScheduleService _service;

        public ScheduleServiceTests() 
        { 
            _service = new ScheduleService(MockUow.Object);
        }

        [Fact]
        [Trait("ScheduleService", "Get Schedule")]
        public void GetSchedule_ReturnsCorrectSchedule()
        {            
            var courseId = Guid.NewGuid();
            var start = DateTime.UtcNow;
            var end = start.AddDays(7);

            var moduleActivity1 = new ModuleActivityDto
            {
                Id = Guid.NewGuid(),
                Name = "Activity 1",
                StartDate = start.AddDays(1),
                EndDate = start.AddDays(2)
            };

            var module1 = new ModuleDto
            {
                Id = Guid.NewGuid(),
                Name = "Module 1",
                StartDate = start,
                EndDate = end,
                ModuleActivities = new List<ModuleActivityDto> { moduleActivity1 }
            };

            var course = new CourseDto
            {
                Id = courseId,
                Name = "Test Course"
            };

            MockCourseService.Setup(s => s.GetCourseByIdAsync(courseId))
                .ReturnsAsync(course);

            MockModuleService.Setup(s => s.GetModulesByCourseIdAsync(courseId))
                .ReturnsAsync(new List<ModuleDto> { module1 });

            
            var schedule = _service.GetSchedule(courseId, start, end);

            
            Assert.NotNull(schedule);
            Assert.Equal(start, schedule.StartDate);
            Assert.Equal(end, schedule.EndDate);
            Assert.Equal(course, schedule.Course);
            Assert.Single(schedule.Modules);
            Assert.Single(schedule.ModuleActivities);
            Assert.Contains(moduleActivity1, schedule.ModuleActivities);
        }
    }
}