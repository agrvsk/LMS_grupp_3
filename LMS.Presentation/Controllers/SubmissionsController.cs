using Microsoft.AspNetCore.Mvc;
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
    public class SubmissionsController: ControllerBase
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

    }
}
