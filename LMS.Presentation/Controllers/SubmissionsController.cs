using LMS.Shared.DTOs.EntityDto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public async Task<IActionResult> CreateSubmission([FromForm] string submissionDtoJson, IFormFile file)
        {
            Console.WriteLine("Received submissionDtoJson: " + submissionDtoJson);
            if (string.IsNullOrEmpty(submissionDtoJson))
                return BadRequest("Submission metadata missing.");

            var submissionDto = JsonConvert.DeserializeObject<SubmissionCreateDto>(submissionDtoJson);
            if (submissionDto == null)
                return BadRequest("Invalid submission data.");

            var createdSubmission = await _serviceManager.SubmissionService
                .CreateSubmissionAsync(submissionDto, file);

            return CreatedAtAction(nameof(GetSubmissionById), new { id = createdSubmission.Id }, createdSubmission);
        }


    }
}
