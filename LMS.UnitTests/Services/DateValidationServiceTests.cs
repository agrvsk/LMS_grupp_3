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
    public class DateValidationServiceTests : ServiceTestBase
    {
        private readonly DateValidationService _service;

        public DateValidationServiceTests()
        {
            _service = new DateValidationService(
                MockMapper.Object,
                MockModuleService.Object,
                MockModuleActivityService.Object,
                MockCourseService.Object);
        }

        #region ValidateCourseDates
        [Fact]
        [Trait("DateValidationService", "Validate Course Dates")]
        public void ValidateCourseDates_ValidDates_ReturnsTrue()
        {
            var start = DateTime.UtcNow;
            var end = start.AddDays(5);

            var result = _service.ValidateCourseDates(start, end);

            Assert.True(result);
        }

        [Fact]
        [Trait("DateValidationService", "Validate Course Dates")]
        public void ValidateCourseDates_StartAfterEnd_ReturnsFalse()
        {
            var start = DateTime.UtcNow.AddDays(5);
            var end = DateTime.UtcNow;

            var result = _service.ValidateCourseDates(start, end);

            Assert.False(result);
        }

        [Fact]
        [Trait("DateValidationService", "Validate Course Dates")]
        public void ValidateCourseDates_DefaultDates_ReturnsFalse()
        {
            var start = default(DateTime);
            var end = default(DateTime);

            var result = _service.ValidateCourseDates(start, end);

            Assert.False(result);
        }
        #endregion

        #region ValidateModuleUppdateDatesAsync
        [Fact]
        [Trait("DateValidationService", "Validate Module Update Dates")]
        public async Task ValidateModuleUppdateDatesAsync_NoModules_ReturnsTrue()
        {
            var courseId = Guid.NewGuid();
            var start = DateTime.UtcNow;
            var end = start.AddDays(5);

            MockModuleService.Setup(s => s.GetModulesByCourseIdAsync(courseId))
                .ReturnsAsync(new List<ModuleDto>());

            var result = await _service.ValidateModuleUppdateDatesAsync(start, end, courseId);

            Assert.True(result);
        }

        [Fact]
        [Trait("DateValidationService", "Validate Module Update Dates")]
        public async Task ValidateModuleUppdateDatesAsync_Overlapping_ReturnsFalse()
        {
            var courseId = Guid.NewGuid();
            var start = DateTime.UtcNow;
            var end = start.AddDays(5);

            MockModuleService.Setup(s => s.GetModulesByCourseIdAsync(courseId))
                .ReturnsAsync(new List<ModuleDto>
                {
                    new ModuleDto
                    {
                        Id = Guid.NewGuid(),
                        StartDate = start.AddDays(3),
                        EndDate = end.AddDays(3)
                    }
                });

            var result = await _service.ValidateModuleUppdateDatesAsync(start, end, courseId);

            Assert.False(result);
        }

        [Fact]
        [Trait("DateValidationService", "Validate Module Update Dates")]
        public async Task ValidateModuleUppdateDatesAsync_DefaultDates_ReturnsFalse()
        {
            var courseId = Guid.NewGuid();

            var result = await _service.ValidateModuleUppdateDatesAsync(default, default, courseId);

            Assert.False(result);
        }
        #endregion

        #region ValidateModuleActivityUppdateDatesAsync
        [Fact]
        [Trait("DateValidationService", "Validate Module Activity Update Dates")]
        public async Task ValidateModuleActivityUppdateDatesAsync_NoActivities_ReturnsTrue()
        {
            var moduleId = Guid.NewGuid();
            var start = DateTime.UtcNow;
            var end = start.AddDays(3);

            MockModuleActivityService.Setup(s => s.GetModuleActivitiesByModuleIdAsync(moduleId))
                .ReturnsAsync(new List<ModuleActivityDto>());

            var result = await _service.ValidateModuleActivityUppdateDatesAsync(start, end, moduleId);

            Assert.True(result);
        }

        [Fact]
        [Trait("DateValidationService", "Validate Module Activity Update Dates")]
        public async Task ValidateModuleActivityUppdateDatesAsync_Overlapping_ReturnsFalse()
        {
            var moduleId = Guid.NewGuid();
            var start = DateTime.UtcNow;
            var end = start.AddDays(5);

            MockModuleActivityService.Setup(s => s.GetModuleActivitiesByModuleIdAsync(moduleId))
                .ReturnsAsync(new List<ModuleActivityDto>
                {
                    new ModuleActivityDto
                    {
                        Id = Guid.NewGuid(),
                        StartDate = start.AddDays(2),
                        EndDate = end.AddDays(2)
                    }
                });

            var result = await _service.ValidateModuleActivityUppdateDatesAsync(start, end, moduleId);

            Assert.False(result);
        }

        [Fact]
        [Trait("DateValidationService", "Validate Module Activity Update Dates")]
        public async Task ValidateModuleActivityUppdateDatesAsync_DefaultDates_ReturnsFalse()
        {
            var moduleId = Guid.NewGuid();

            var result = await _service.ValidateModuleActivityUppdateDatesAsync(default, default, moduleId);

            Assert.False(result);
        }
        #endregion

        #region ValidateListOfDateRanges
        [Fact]
        [Trait("DateValidationService", "Validate List Of Date Ranges")]
        public void ValidateListOfDateRanges_ValidRanges_ReturnsTrue()
        {
            var minDate = DateTime.UtcNow;
            var maxDate = minDate.AddDays(10);

            var ranges = new List<(DateTime startDate, DateTime endDate)>
            {
                (minDate.AddDays(0), minDate.AddDays(2)),
                (minDate.AddDays(3), minDate.AddDays(5))
            };

            var result = _service.ValidateListOfDateRanges(ranges, minDate, maxDate);

            Assert.True(result);
        }

        [Fact]
        [Trait("DateValidationService", "Validate List Of Date Ranges")]
        public void ValidateListOfDateRanges_OverlappingRanges_ReturnsFalse()
        {
            var minDate = DateTime.UtcNow;
            var maxDate = minDate.AddDays(10);

            var ranges = new List<(DateTime startDate, DateTime endDate)>
            {
                (minDate.AddDays(0), minDate.AddDays(4)),
                (minDate.AddDays(3), minDate.AddDays(5))
            };

            var result = _service.ValidateListOfDateRanges(ranges, minDate, maxDate);

            Assert.False(result);
        }

        [Fact]
        [Trait("DateValidationService", "Validate List Of Date Ranges")]
        public void ValidateListOfDateRanges_OutOfBounds_ReturnsFalse()
        {
            var minDate = DateTime.UtcNow;
            var maxDate = minDate.AddDays(5);

            var ranges = new List<(DateTime startDate, DateTime endDate)>
            {
                (minDate.AddDays(0), minDate.AddDays(2)),
                (minDate.AddDays(4), minDate.AddDays(6))
            };

            var result = _service.ValidateListOfDateRanges(ranges, minDate, maxDate);

            Assert.False(result);
        }

        [Fact]
        [Trait("DateValidationService", "Validate List Of Date Ranges")]
        public void ValidateListOfDateRanges_DefaultDates_ReturnsFalse()
        {
            var minDate = DateTime.UtcNow;
            var maxDate = minDate.AddDays(10);

            var ranges = new List<(DateTime startDate, DateTime endDate)>
            {
                (default, default),
            };

            var result = _service.ValidateListOfDateRanges(ranges, minDate, maxDate);

            Assert.False(result);
        }

        [Fact]
        [Trait("DateValidationService", "Validate List Of Date Ranges")]
        public void ValidateListOfDateRanges_EmptyList_ReturnsTrue()
        {
            var minDate = DateTime.UtcNow;
            var maxDate = minDate.AddDays(10);

            var ranges = new List<(DateTime startDate, DateTime endDate)>();

            var result = _service.ValidateListOfDateRanges(ranges, minDate, maxDate);

            Assert.True(result);
        }
        #endregion
    }
}