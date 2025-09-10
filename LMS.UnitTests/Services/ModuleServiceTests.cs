using Domain.Models.Entities;
using Domain.Models.Exceptions;
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
    public class ModuleServiceTests : ServiceTestBase
    {
        private readonly ModuleService _service;

        public ModuleServiceTests()
        {            
            _service = new ModuleService(MockUow.Object, MockMapper.Object);
        }

        [Fact]
        [Trait("ModuleService", "Get All Modules")]
        public async Task GetAllModulesAsync_ReturnsAllModules()
        {
            var modules = new List<Module>
            {
                new() { Id = Guid.NewGuid(), Name = "TestModule" }
            };

            var moduleDtos = new List<ModuleDto>
            {
                new() { Id = modules.First().Id, Name = "TestModule" }
            };

            MockUow.Setup(u => u.ModuleRepository.GetAllModulesAsync())
                            .ReturnsAsync(modules);

            MockMapper.Setup(m => m.Map<List<ModuleDto>>(modules))
                            .Returns(moduleDtos);

            var result = await _service.GetAllModulesAsync();

            Assert.Single(result);
            Assert.Equal("TestModule", result.First().Name);
        }

        [Fact]
        [Trait("ModuleService", "Get All Modules")]
        public async Task GetAllModulesAsync_NoModules_ReturnsEmptyList()
        {
            var modules = new List<Module>();
            var moduleDtos = new List<ModuleDto>();

            MockUow.Setup(u => u.ModuleRepository.GetAllModulesAsync())
                   .ReturnsAsync(new List<Module>());

            MockMapper.Setup(m => m.Map<List<ModuleDto>>(modules))
                            .Returns(moduleDtos);

            var result = await _service.GetAllModulesAsync();

            Assert.Empty(result);
        }

        [Fact]
        [Trait("ModuleService", "Get Module By Id")]
        public async Task GetModuleByIdAsync_ValidId_ReturnsModule()
        {
            var moduleId = Guid.NewGuid();
            var module = new Module { Id = moduleId, Name = "ModuleTester" };
            var moduledto = new ModuleDto { Id = moduleId, Name = "ModuleTester" };

            MockUow.Setup(u => u.ModuleRepository.GetModuleByIdAsync(moduleId))
                            .ReturnsAsync(module);

            MockMapper.Setup(m => m.Map<ModuleDto>(It.IsAny<Module>()))
                            .Returns(moduledto);
                        

            var result = await _service.GetModuleByIdAsync(moduleId);

            Assert.NotNull(result);
            Assert.Equal(moduleId, result.Id);
            Assert.Equal("ModuleTester", result.Name);
        }

        [Fact]
        [Trait("ModuleService", "Get Module By Id")]
        public async Task GetModuleByIdAsync_InvalidId_ThrowsException()
        {
            var id = Guid.NewGuid();

            MockUow.Setup(u => u.ModuleRepository.GetModuleByIdAsync(id))
                   .ReturnsAsync((Module?)null);

            await Assert.ThrowsAsync<ModuleNotFoundException>(() => _service.GetModuleByIdAsync(id));
        }

        [Fact]
        [Trait("ModuleService", "Get Modules By Course Id")]
        public async Task GetModulesByCourseIdAsync_ValidCourseId_ReturnsModules()
        {
            var courseId = Guid.NewGuid();
            var modules = new List<Module>
            {
                new() { Id = Guid.NewGuid(), Name = "Course Module", CourseId = courseId }
            };

            var moduleDtos = new List<ModuleDto>
            {
                new() { Id = modules.First().Id, Name = "Course Module" }
            };

            MockUow.Setup(u => u.ModuleRepository.GetModulesByCourseIdAsync(courseId))
                   .ReturnsAsync(modules);

            MockMapper.Setup(m => m.Map<List<ModuleDto>>(modules))
                            .Returns(moduleDtos);

            var result = await _service.GetModulesByCourseIdAsync(courseId);

            Assert.Single(result);
            Assert.Equal("Course Module", result.First().Name);
        }

        [Fact]
        [Trait("ModuleService", "Create Module")]
        public async Task CreateModuleAsync_ValidDto_CallsCreateAndSaves()
        {
            var dto = new ModuleCreateDto { Name = "Created Module" };
            var mappedModule = new Module { Id = Guid.NewGuid(), Name = dto.Name };

            MockMapper.Setup(m => m.Map<Module>(dto)).Returns(mappedModule);

            var result = await _service.CreateModuleAsync(dto);

            Assert.Equal(mappedModule, result);
            MockUow.Verify(u => u.ModuleRepository.Create(mappedModule), Times.Once);
            MockUow.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Fact]
        [Trait("ModuleService", "Update Module")]
        public async Task UpdateModuleAsync_ValidDto_CallsUpdateAndSaves()
        {
            var dto = new ModuleUpdateDto { Id = Guid.NewGuid(), Name = "Updated Module" };
            var mappedModule = new Module { Id = dto.Id, Name = dto.Name };

            MockMapper.Setup(m => m.Map<Module>(dto)).Returns(mappedModule);

            var result = await _service.UpdateModuleAsync(dto);

            //Assert.Equal(mappedModule, result);
            MockUow.Verify(u => u.ModuleRepository.Update(mappedModule), Times.Once);
            MockUow.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Fact]
        [Trait("ModuleService", "Delete Module")]
        public async Task DeleteModuleAsync_ExistingModule_DeletesAndSaves()
        {
            var id = Guid.NewGuid();
            var module = new Module { Id = id };

            MockModuleRepo.Setup(r => r.GetModuleByIdAsync(id))
                          .ReturnsAsync(module);

            var result = await _service.DeleteModuleAsync(id);

            Assert.True(result);
            MockUow.Verify(u => u.ModuleRepository.Delete(module), Times.Once);
            MockUow.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Fact]
        [Trait("ModuleService", "Delete Module")]
        public async Task DeleteModuleAsync_NonExistingModule_ReturnsFalse()
        {
            var id = Guid.NewGuid();
            MockUow.Setup(u => u.ModuleRepository.GetModuleByIdAsync(id))
                   .ReturnsAsync((Module?)null);


            var result = await _service.DeleteModuleAsync(id);

            Assert.False(result);
            MockUow.Verify(u => u.ModuleRepository.Delete(It.IsAny<Module>()), Times.Never);
            MockUow.Verify(u => u.CompleteAsync(), Times.Never);
        }

    }
}
