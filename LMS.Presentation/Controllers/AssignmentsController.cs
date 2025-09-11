using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Presentation.Controllers
{

    [Route("/assignments")]
    [ApiController]
    public class AssignmentsController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        public AssignmentsController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAssignmentById(Guid id)
        {
            var assignment = _serviceManager.AssignmentService.GetAssignmentById(id);
            return Ok(assignment.Result);
        }
        [HttpGet("activity/{activityId}")]
        public async Task<IActionResult> GetAssignmentsByActivityId(Guid activityId)
        {
            var assignments = _serviceManager.AssignmentService.GetAssignmentsByActivityId(activityId);
            return Ok(assignments.Result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAssignments()
        {
            var assignments = await
                _serviceManager.AssignmentService.GetAllAssignmentsAsync();
            return Ok(assignments);
        }
    }
}
