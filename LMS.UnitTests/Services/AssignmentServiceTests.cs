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
    public class AssignmentServiceTests : ServiceTestBase
    {
        protected readonly AssignmentService _service;

        public AssignmentServiceTests()
        {
            _service = new AssignmentService(MockUow.Object, MockMapper.Object);
        }

        [Fact]
        [Trait("AssignmentServiceTests", "Get Assignment")]
        public async Task GetAssignmentById_AssignmentExists_ReturnsDto()
        {
            var assignmentId = Guid.NewGuid();
            var assignment = new Assignment { Id = assignmentId, Name = "Test Assignment" };
            var assignmentDto = new AssignmentDto { Id = assignmentId, Name = "Test Assignment" };

            MockAssignmentRepo
                .Setup(r => r.GetAssignmentByIdAsync(assignmentId))
                .ReturnsAsync(assignment);

            MockMapper
                .Setup(m => m.Map<AssignmentDto>(assignment))
                .Returns(assignmentDto);

            var result = await _service.GetAssignmentById(assignmentId);

            Assert.NotNull(result);
            Assert.Equal(assignmentId, result.Id);
            Assert.Equal("Test Assignment", result.Name);

            MockAssignmentRepo.Verify(r => r.GetAssignmentByIdAsync(assignmentId), Times.Once);
            MockMapper.Verify(m => m.Map<AssignmentDto>(assignment), Times.Once);
        }

        [Fact]
        [Trait("AssignmentServiceTests", "Get Assignment")]
        public async Task GetAssignmentById_AssignmentDoesNotExist_ThrowsException()
        {
            var assignmentId = Guid.NewGuid();
            MockAssignmentRepo
                .Setup(r => r.GetAssignmentByIdAsync(assignmentId))
                .ReturnsAsync((Assignment?)null);
                     
            await Assert.ThrowsAsync<Exception>(() => _service.GetAssignmentById(assignmentId));

            MockAssignmentRepo.Verify(r => r.GetAssignmentByIdAsync(assignmentId), Times.Once);
            MockMapper.Verify(m => m.Map<AssignmentDto>(It.IsAny<Assignment>()), Times.Never);
        }
    }
}  