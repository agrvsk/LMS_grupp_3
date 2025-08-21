using LMS.UnitTests.Setups;
using LMS.Shared.DTOs.EntityDto;
using LMS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models.Entities;
using Moq;

namespace LMS.UnitTests.Services
{
    public class CourseServiceTests : ServiceTestBase
    {

        private readonly CourseService _service;

        public CourseServiceTests()
        {
            MockUow.Setup(u => u.CompleteAsync()).Returns(Task.CompletedTask);
            _service = new CourseService(MockUow.Object, MockMapper.Object);            
        }

        #region [GetCourseByIdAsync]
        [Fact]
        [Trait("CourseService", "Get Course")]
        public async Task GetCourseByIdAsync_CourseExists_ReturnsCourse()
        {
            var courseId = Guid.NewGuid();
            var course = new Course { Id = courseId, Name = "TestCourse" };

            MockCourseRepo
                .Setup(r => r.GetCourseByIdAsync(courseId))
                .ReturnsAsync(course);

            var result = await _service.GetCourseByIdAsync(courseId);

            Assert.NotNull(result);
            Assert.Equal(courseId, result.Id);
        }

        [Fact]
        [Trait("CourseService", "Get Course")]
        public async Task GetCourseByIdAsync_CourseDoesNotExist_ReturnsNull()
        {
            var courseId = Guid.NewGuid();

            MockCourseRepo
                .Setup(r => r.GetCourseByIdAsync(courseId))
                .ReturnsAsync((Course?)null);

            var result = await _service.GetCourseByIdAsync(courseId);

            Assert.Null(result);
        }
        #endregion

        #region [GetAllCoursesAsync]
        [Fact]
        [Trait("CourseService", "Get All Courses")]
        public async Task GetAllCoursesAsync_CoursesExist_ReturnsList()
        {
            var courses = new List<Course>
            {
                new Course { Id = Guid.NewGuid(), Name = "Course1" },
                new Course { Id = Guid.NewGuid(), Name = "Course2" }
            };

            MockCourseRepo.Setup(r => r.GetAllCoursesAsync()).ReturnsAsync(courses);

            var result = await _service.GetAllCoursesAsync();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        [Trait("CourseService", "Get All Courses")]
        public async Task GetAllCoursesAsync_NoCourses_ReturnsEmptyList()
        {
            MockCourseRepo.Setup(r => r.GetAllCoursesAsync()).ReturnsAsync(new List<Course>());

            var result = await _service.GetAllCoursesAsync();

            Assert.NotNull(result);
            Assert.Empty(result);
        }
        #endregion

        #region [CreateCourseAsync]
        [Fact]
        [Trait("CourseService", "Create Course")]
        public async Task CreateCourseAsync_ValidDto_AddsNewCourse()
        {
            var courseDto = new CourseCreateDto { Name = "TestCourse", Description = "Lorem Ipsum" };
            var course = new Course { Id = Guid.NewGuid(), Name = "TestCourse", Description = "Lorem Ipsum" };

            MockMapper
                .Setup(m => m.Map<Course>(courseDto))
                .Returns(course);

            var result = await _service.CreateCourseAsync(courseDto);

            Assert.NotNull(result);
            Assert.Equal("TestCourse", result.Name);
            MockCourseRepo.Verify(r => r.Create(It.IsAny<Course>()), Times.Once);
            MockUow.Verify(u => u.CompleteAsync(), Times.Once);
        }
        #endregion

        #region [UpdateCourseAsync]
        [Fact]
        [Trait("CourseService", "Update Course")]
        public async Task UpdateCourseAsync_ValidDto_CallsUpdatesAndSaves()
        {
            var dto = new CourseDto { Id = Guid.NewGuid(), Name = "Updated" };
            var mappedCourse = new Course { Id = dto.Id, Name = dto.Name };

            MockMapper
                .Setup(m => m.Map<Course>(dto))
                .Returns(mappedCourse);

            var service = new CourseService(MockUow.Object, MockMapper.Object);

            var result = await _service.UpdateCourseAsync(dto);

            // Verify that the mapper was called correctly
            //Assert.Equal(mappedCourse, result);

            // Verify that the repository methods were called once
            MockCourseRepo.Verify(r => r.Update(mappedCourse), Times.Once);

            // Verify that the unit of work's CompleteAsync was called once
            MockUow.Verify(u => u.CompleteAsync(), Times.Once);            
        }
        #endregion

        #region [DeleteCourseAsync]
        [Fact]
        [Trait("CourseService", "Delete Course")]
        public async Task DeleteCourseAsync_WhenCourseDoesNotExist_ReturnsFalse()
        {
            var courseId = Guid.NewGuid();

            MockCourseRepo
                .Setup(r => r.GetCourseByIdAsync(courseId))
                .ReturnsAsync((Course?)null);

            var result = await _service.DeleteCourseAsync(courseId);

            MockCourseRepo.Verify(r => r.Delete(It.IsAny<Course>()), Times.Never);
            MockUow.Verify(u => u.CompleteAsync(), Times.Never);

            Assert.False(result);            
        }

        [Fact]
        [Trait("CourseService", "Delete Course")]
        public async Task DeleteCourseAsync_WhenCourseExists_DeletesAndReturnsTrue()
        {
            var courseId = Guid.NewGuid();
            var course = new Course { Id = courseId };

            MockCourseRepo
                .Setup(r => r.GetCourseByIdAsync(courseId))
                .ReturnsAsync(course);

            var result = await _service.DeleteCourseAsync(courseId);

            MockCourseRepo.Verify(r => r.Delete(course), Times.Once);
            MockUow.Verify(u => u.CompleteAsync(), Times.Once);

            Assert.True(result);
        }
        #endregion
    }
}
