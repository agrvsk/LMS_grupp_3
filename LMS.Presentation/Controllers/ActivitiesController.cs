using LMS.Shared.DTOs.EntityDto;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Presentation.Controllers
{
    [Route("/activities")]
    [ApiController]
    public class ActivitiesController: ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        public ActivitiesController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetActivities()
        {
            var activities = await _serviceManager.ModuleActivityService.GetAllModuleActivitiesAsync();
            return Ok(activities);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetActivityById(string id)
        {
            var activity = await _serviceManager.ModuleActivityService.GetModuleActivityByIdAsync(id);
            if (activity == null)
                return NotFound();
            return Ok(activity);
        }
        [HttpPost]
        public async Task<IActionResult> CreateActivity([FromBody] ModuleActivityCreateDto activityDto)
        {
            if (activityDto == null)
                return BadRequest("Activity data is null");
            var createdActivity = await _serviceManager.ModuleActivityService.CreateActivityAsync(activityDto);
            return CreatedAtAction(nameof(GetActivityById), new { id = createdActivity.Id }, createdActivity);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateActivity(string id, [FromBody] ModuleActivityDto activityDto)
        {
            if (activityDto == null || id != activityDto.Id)
                return BadRequest("Activity data is invalid");
            var updatedActivity = await _serviceManager.ModuleActivityService.UpdateActivityAsync(activityDto);
            if (updatedActivity == null)
                return NotFound();
            return Ok(updatedActivity);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActivity(string id)
        {
            var deleted = await _serviceManager.ModuleActivityService.DeleteActivityAsync(id);
            if (!deleted)
                return NotFound();
            return NoContent();
        }
    }
}
