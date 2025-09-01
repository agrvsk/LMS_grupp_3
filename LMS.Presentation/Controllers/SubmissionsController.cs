using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.Shared.DTOs.EntityDto;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;

namespace LMS.Presentation.Controllers
{
    [Route("/submissions")]
    [ApiController]
    public class SubmissionsController : ControllerBase
    {
        public readonly IServiceManager _serviceManager;
        public SubmissionsController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }
        [HttpGet]
        public async Task<IActionResult> GetSubmissions()
        {
            var submissions = await _serviceManager.SubmissionService.GetAllSubmissionsAsync();
            return Ok(submissions);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSubmissionById(Guid id)
        {
            var submission = await _serviceManager.SubmissionService.GetSubmissionByIdAsync(id);
            if (submission == null)
                return NotFound();
            return Ok(submission);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetSubmissionByUserId(string userId)
        {
            var submission = await _serviceManager.SubmissionService.GetSubmissionsByApplicationUserIdAsync(userId);
            if (submission == null)
                return NotFound();
            return Ok(submission);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSubmission([FromBody] SubmissionCreateDto submissionCreateDto)
        {
            if (submissionCreateDto == null)
                return BadRequest("Submission data is null");

            var createdSubmission = await _serviceManager.SubmissionService.CreateSubmissionAsync(submissionCreateDto);
            return CreatedAtAction(nameof(GetSubmissionById), new { id = createdSubmission.Id }, createdSubmission );
        }


    }
}
