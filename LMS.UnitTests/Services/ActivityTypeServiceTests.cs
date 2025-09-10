using Domain.Models.Entities;
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
    public class ActivityTypeServiceTests : ServiceTestBase
    {
        protected readonly ActivityTypeService _service;

        public ActivityTypeServiceTests()
        {
            _service = new ActivityTypeService(MockUow.Object, MockMapper.Object);
        }

        [Fact]
        [Trait("ActivityTypeService", "Get All Activity Types")]
        public async Task GetAllActivityTypesAsync_TypesExist_ReturnsMappedDtos()
        {
            var activityTypes = new List<ActivityType>
            {
                new ActivityType { Id = 1, Name = "Quiz" },
                new ActivityType { Id = 2, Name = "Assignment" }
            };

            var activityTypeDtos = new List<ActivityTypeDto>
            {
                new ActivityTypeDto { Id = activityTypes[0].Id, Name = "Quiz" },
                new ActivityTypeDto { Id = activityTypes[1].Id, Name = "Assignment" }
            };

            MockActivityTypeRepo
                .Setup(r => r.GetAllActivityTypesAsync())
                .ReturnsAsync(activityTypes);

            MockMapper
                .Setup(m => m.Map<List<ActivityTypeDto>>(activityTypes))
                .Returns(activityTypeDtos);

            var result = await _service.GetAllActivityTypesAsync();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("Quiz", result[0].Name);
            Assert.Equal("Assignment", result[1].Name);

            MockActivityTypeRepo.Verify(r => r.GetAllActivityTypesAsync(), Times.Once);
            MockMapper.Verify(m => m.Map<List<ActivityTypeDto>>(activityTypes), Times.Once);
        }

        [Fact]
        [Trait("ActivityTypeService", "Get All Activity Types")]
        public async Task GetAllActivityTypesAsync_NoTypes_ReturnsEmptyList()
        {            
            var emptyList = new List<ActivityType>();
            var emptyDtoList = new List<ActivityTypeDto>();

            MockActivityTypeRepo
                .Setup(r => r.GetAllActivityTypesAsync())
                .ReturnsAsync(emptyList);

            MockMapper
                .Setup(m => m.Map<List<ActivityTypeDto>>(emptyList))
                .Returns(emptyDtoList);

            
            var result = await _service.GetAllActivityTypesAsync();

            
            Assert.NotNull(result);
            Assert.Empty(result);

            MockActivityTypeRepo.Verify(r => r.GetAllActivityTypesAsync(), Times.Once);
            MockMapper.Verify(m => m.Map<List<ActivityTypeDto>>(emptyList), Times.Once);
        }

        [Fact]
        [Trait("ActivityTypeService", "Get All Activity Types")]
        public async Task GetAllActivityTypesAsync_RepositoryThrows_ThrowsException()
        {
            MockActivityTypeRepo
                .Setup(r => r.GetAllActivityTypesAsync())
                .ThrowsAsync(new Exception("Database error"));

            await Assert.ThrowsAsync<Exception>(() => _service.GetAllActivityTypesAsync());

            MockActivityTypeRepo.Verify(r => r.GetAllActivityTypesAsync(), Times.Once);
            MockMapper.Verify(m => m.Map<List<ActivityTypeDto>>(It.IsAny<List<ActivityType>>()), Times.Never);
        }        
    }
}