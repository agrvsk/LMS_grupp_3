using Domain.Models.Entities;
using LMS.Services;
using LMS.UnitTests.Setups;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.UnitTests.Services
{
    public class SubmissionServiceTests : ServiceTestBase
    {
        private readonly SubmissionService _service;

        //public SubmissionServiceTests()
        //{
        //    _service = new SubmissionService(MockUow.Object);
        //}

        //#region [GetAllSubmissionsAsync]
        //[Fact]
        //[Trait("SubmissionService", "Get All Submissions")]
        //public async Task GetAllSubmissionsAsync_SubmissionsExist_ReturnsList()
        //{
        //    var submissions = new List<Submission>
        //    {
        //        new Submission { Id = Guid.NewGuid(), ApplicationUserId = "user1" },
        //        new Submission { Id = Guid.NewGuid(), ApplicationUserId = "user2" }
        //    };

        //    MockSubmissionRepo
        //        .Setup(r => r.GetAllSubmissionsAsync())
        //        .ReturnsAsync(submissions);

        //    var result = await _service.GetAllSubmissionsAsync();

        //    Assert.NotNull(result);
        //    Assert.Equal(2, result.Count);
        //}

        //[Fact]
        //[Trait("SubmissionService", "Get All Submissions")]
        //public async Task GetAllSubmissionsAsync_NoSubmissions_ReturnsEmptyList()
        //{
        //    var emptyList = new List<Submission>();

        //    MockSubmissionRepo
        //        .Setup(r => r.GetAllSubmissionsAsync())
        //        .ReturnsAsync(emptyList);

        //    var result = await _service.GetAllSubmissionsAsync();

        //    Assert.NotNull(result);
        //    Assert.Empty(result);
        //}
        //#endregion

        //#region [GetSubmissionByIdAsync]
        //[Fact]
        //[Trait("SubmissionService", "Get Submission By Id")]
        //public async Task GetSubmissionByIdAsync_SubmissionExists_ReturnsSubmission()
        //{
        //    var submissionId = Guid.NewGuid();
        //    var submission = new Submission { Id = submissionId, ApplicationUserId = "user1" };

        //    MockSubmissionRepo
        //        .Setup(r => r.GetSubmissionByIdAsync(submissionId))
        //        .ReturnsAsync(submission);

        //    var result = await _service.GetSubmissionByIdAsync(submissionId);

        //    Assert.NotNull(result);
        //    Assert.Equal(submissionId, result!.Id);
        //}

        //[Fact]
        //[Trait("SubmissionService", "Get Submission By Id")]
        //public async Task GetSubmissionByIdAsync_SubmissionDoesNotExist_ReturnsNull()
        //{
        //    var submissionId = Guid.NewGuid();

        //    MockSubmissionRepo
        //        .Setup(r => r.GetSubmissionByIdAsync(submissionId))
        //        .ReturnsAsync((Submission?)null);

        //    var result = await _service.GetSubmissionByIdAsync(submissionId);

        //    Assert.Null(result);
        //}
        //#endregion

        //#region [GetSubmissionsByApplicationUserIdAsync]
        //[Fact]
        //[Trait("SubmissionService", "Get Submissions By UserId")]
        //public async Task GetSubmissionsByApplicationUserIdAsync_SubmissionsExist_ReturnsList()
        //{
        //    var userId = "user1";
        //    var submissions = new List<Submission>
        //    {
        //        new Submission { Id = Guid.NewGuid(), ApplicationUserId = userId }
        //    };

        //    MockSubmissionRepo
        //        .Setup(r => r.GetSubmissionsByApplicationUserIdAsync(userId))
        //        .ReturnsAsync(submissions);

        //    var result = await _service.GetSubmissionsByApplicationUserIdAsync(userId);

        //    Assert.NotNull(result);
        //    Assert.Single(result);
        //    Assert.Equal(userId, result[0].ApplicationUserId);
        //}

        //[Fact]
        //[Trait("SubmissionService", "Get Submissions By UserId")]
        //public async Task GetSubmissionsByApplicationUserIdAsync_NoSubmissions_ReturnsEmptyList()
        //{
        //    var userId = "user1";

        //    MockSubmissionRepo
        //        .Setup(r => r.GetSubmissionsByApplicationUserIdAsync(userId))
        //        .ReturnsAsync(new List<Submission>());

        //    var result = await _service.GetSubmissionsByApplicationUserIdAsync(userId);

        //    Assert.NotNull(result);
        //    Assert.Empty(result);
        //}
        //#endregion

        //#region [GetSubmissionsByDocumentIdAsync]
        //[Fact]
        //[Trait("SubmissionService", "Get Submissions By DocumentId")]
        //public async Task GetSubmissionsByDocumentIdAsync_SubmissionsExist_ReturnsList()
        //{
        //    var docId = Guid.NewGuid();
        //    var submissions = new List<Submission>
        //    {
        //        new Submission { Id = Guid.NewGuid(), DocumentId = docId }
        //    };

        //    MockSubmissionRepo
        //        .Setup(r => r.GetSubmissionsByDocumentIdAsync(docId))
        //        .ReturnsAsync(submissions);

        //    var result = await _service.GetSubmissionsByDocumentIdAsync(docId);

        //    Assert.NotNull(result);
        //    Assert.Single(result);
        //    Assert.Equal(docId, result[0].DocumentId);
        //}

        //[Fact]
        //[Trait("SubmissionService", "Get Submissions By DocumentId")]
        //public async Task GetSubmissionsByDocumentIdAsync_NoSubmissions_ReturnsEmptyList()
        //{
        //    var docId = Guid.NewGuid();

        //    MockSubmissionRepo
        //        .Setup(r => r.GetSubmissionsByDocumentIdAsync(docId))
        //        .ReturnsAsync(new List<Submission>());

        //    var result = await _service.GetSubmissionsByDocumentIdAsync(docId);

        //    Assert.NotNull(result);
        //    Assert.Empty(result);
        //}
        //#endregion
    }
}
