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
    public class ModuleActivityServiceTests : ServiceTestBase
    {
        private readonly ModuleActivityService _service;

        public ModuleActivityServiceTests()
        {           
            _service = new ModuleActivityService(MockUow.Object, MockMapper.Object);
        }

        #region [GetModuleActivityByIdAsync]
        [Fact]
        [Trait("ModuleActivityService", "Get By Id")]
        public async Task GetModuleActivityByIdAsync_ActivityExists_ReturnsDto()
        {
            var id = Guid.NewGuid();
            var activity = new ModuleActivity { Id = id, Name = "TestActivity" };
            var dto = new ModuleActivityDto { Id = id, Name = "TestActivity" };

            MockModuleActivityRepo
                .Setup(r => r.GetModuleActivityByIdAsync(id))
                .ReturnsAsync(activity);

            MockMapper
                .Setup(m => m.Map<ModuleActivityDto>(activity))
                .Returns(dto);

            var result = await _service.GetModuleActivityByIdAsync(id);

            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
            Assert.Equal("TestActivity", result.Name);
        }

        [Fact]
        [Trait("ModuleActivityService", "Get By Id")]
        public async Task GetModuleActivityByIdAsync_ActivityDoesNotExist_ReturnsNull()
        {
            var id = Guid.NewGuid();

            MockModuleActivityRepo
                .Setup(r => r.GetModuleActivityByIdAsync(id))
                .ReturnsAsync((ModuleActivity?)null);

            MockMapper
                .Setup(m => m.Map<ModuleActivityDto>(null))
                .Returns((ModuleActivityDto?)null);

            var result = await _service.GetModuleActivityByIdAsync(id);

            Assert.Null(result);
        }
        #endregion

        #region [GetAllModuleActivitiesAsync]
        [Fact]
        [Trait("ModuleActivityService", "Get All")]
        public async Task GetAllModuleActivitiesAsync_ActivitiesExist_ReturnsList()
        {
            var activities = new List<ModuleActivity>
            {
                new() { Id = Guid.NewGuid(), Name = "Activity1" },
                new() { Id = Guid.NewGuid(), Name = "Activity2" }
            };

            var dtos = activities.Select(a => new ModuleActivityDto { Id = a.Id, Name = a.Name }).ToList();

            MockModuleActivityRepo
                .Setup(r => r.GetAllModuleActivitiesAsync())
                .ReturnsAsync(activities);

            MockMapper
                .Setup(m => m.Map<List<ModuleActivityDto>>(activities))
                .Returns(dtos);

            var result = await _service.GetAllModuleActivitiesAsync();

            Assert.Equal(2, result.Count);
        }

        [Fact]
        [Trait("ModuleActivityService", "Get All")]
        public async Task GetAllModuleActivitiesAsync_NoActivities_ReturnsEmptyList()
        {
            MockModuleActivityRepo
                .Setup(r => r.GetAllModuleActivitiesAsync())
                .ReturnsAsync(new List<ModuleActivity>());

            MockMapper
                .Setup(m => m.Map<List<ModuleActivityDto>>(It.IsAny<List<ModuleActivity>>()))
                .Returns(new List<ModuleActivityDto>());

            var result = await _service.GetAllModuleActivitiesAsync();

            Assert.Empty(result);
        }
        #endregion

        #region [GetModuleActivitiesByModuleIdAsync]
        [Fact]
        [Trait("ModuleActivityService", "Get By Module Id")]
        public async Task GetModuleActivitiesByModuleIdAsync_ValidModuleId_ReturnsActivities()
        {
            var moduleId = Guid.NewGuid();
            var activities = new List<ModuleActivity>
            {
                new() { Id = Guid.NewGuid(), Name = "Activity1", ModuleId = moduleId }
            };

            var dtos = new List<ModuleActivityDto>
            {
                new() { Id = activities[0].Id, Name = "Activity1" }
            };

            MockModuleActivityRepo
                .Setup(r => r.GetModuleActivitiesByModuleIdAsync(moduleId))
                .ReturnsAsync(activities);

            MockMapper
                .Setup(m => m.Map<List<ModuleActivityDto>>(activities))
                .Returns(dtos);

            var result = await _service.GetModuleActivitiesByModuleIdAsync(moduleId);

            Assert.Single(result);
            Assert.Equal("Activity1", result.First().Name);
        }
        #endregion

        #region [CreateActivityAsync]
        [Fact]
        [Trait("ModuleActivityService", "Create")]
        public async Task CreateActivityAsync_ValidDto_AddsNewActivity()
        {
            var createDto = new ModuleActivityCreateDto { Name = "NewActivity" };
            var activity = new ModuleActivity { Id = Guid.NewGuid(), Name = "NewActivity" };

            MockMapper
                .Setup(m => m.Map<ModuleActivity>(createDto))
                .Returns(activity);

            var result = await _service.CreateActivityAsync(createDto);

            Assert.NotNull(result);
            Assert.Equal("NewActivity", result.Name);
            MockModuleActivityRepo.Verify(r => r.Create(activity), Times.Once);
            MockUow.Verify(u => u.CompleteAsync(), Times.Once);
        }
        #endregion

        #region [UpdateActivityAsync]
        [Fact]
        [Trait("ModuleActivityService", "Update")]
        public async Task UpdateActivityAsync_ActivityExists_UpdatesAndReturnsEntity()
        {
            var dto = new ModuleActivityDto { Id = Guid.NewGuid(), Name = "UpdatedActivity" };
            var existing = new ModuleActivity { Id = dto.Id, Name = "OldName" };

            MockModuleActivityRepo
                .Setup(r => r.GetModuleActivityByIdAsync(dto.Id))
                .ReturnsAsync(existing);

            MockMapper
                .Setup(m => m.Map(dto, existing))
                .Callback(() => existing.Name = dto.Name);

            var result = await _service.UpdateActivityAsync(dto);

            Assert.NotNull(result);
            Assert.Equal("UpdatedActivity", result.Name);
            MockModuleActivityRepo.Verify(r => r.Update(existing), Times.Once);
            MockUow.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Fact]
        [Trait("ModuleActivityService", "Update")]
        public async Task UpdateActivityAsync_ActivityDoesNotExist_ReturnsNull()
        {
            var dto = new ModuleActivityDto { Id = Guid.NewGuid(), Name = "DoesNotExist" };

            MockModuleActivityRepo
                .Setup(r => r.GetModuleActivityByIdAsync(dto.Id))
                .ReturnsAsync((ModuleActivity?)null);

            var result = await _service.UpdateActivityAsync(dto);

            Assert.Null(result);
            MockModuleActivityRepo.Verify(r => r.Update(It.IsAny<ModuleActivity>()), Times.Never);
            MockUow.Verify(u => u.CompleteAsync(), Times.Never);
        }
        #endregion

        #region [DeleteActivityAsync]
        [Fact]
        [Trait("ModuleActivityService", "Delete")]
        public async Task DeleteActivityAsync_ActivityExists_DeletesAndReturnsTrue()
        {
            var id = Guid.NewGuid();
            var activity = new ModuleActivity { Id = id };

            MockModuleActivityRepo
                .Setup(r => r.GetModuleActivityByIdAsync(id))
                .ReturnsAsync(activity);

            var result = await _service.DeleteActivityAsync(id);

            Assert.True(result);
            MockModuleActivityRepo.Verify(r => r.Delete(activity), Times.Once);
            MockUow.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Fact]
        [Trait("ModuleActivityService", "Delete")]
        public async Task DeleteActivityAsync_ActivityDoesNotExist_ReturnsFalse()
        {
            var id = Guid.NewGuid();

            MockModuleActivityRepo
                .Setup(r => r.GetModuleActivityByIdAsync(id))
                .ReturnsAsync((ModuleActivity?)null);

            var result = await _service.DeleteActivityAsync(id);

            Assert.False(result);
            MockModuleActivityRepo.Verify(r => r.Delete(It.IsAny<ModuleActivity>()), Times.Never);
            MockUow.Verify(u => u.CompleteAsync(), Times.Never);
        }
        #endregion
    }
}
