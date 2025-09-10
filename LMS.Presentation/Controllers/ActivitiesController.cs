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
        [HttpGet]
        [Route("module/{moduleId}")]
        public async Task<IActionResult> GetActivitiesByModuleId(Guid moduleId)
        {
            var activities = await _serviceManager.ModuleActivityService.GetModuleActivitiesByModuleIdAsync(moduleId);
            return Ok(activities);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetActivityById(Guid id)
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
            if (!await _serviceManager.DateValidationService.ValidateModuleActivityUppdateDatesAsync(activityDto.StartDate, activityDto.EndDate, activityDto.ModuleId))
            {
                ModelState.AddModelError("DateValidation", "Aktiviteter får inte överlappa och starttid måste vara innan sluttid.");
                return BadRequest(ModelState);
            }
                var createdActivity = await _serviceManager.ModuleActivityService.CreateActivityAsync(activityDto);
            return CreatedAtAction(nameof(GetActivityById), new { id = createdActivity.Id }, createdActivity);
        }

        [HttpPost("with-documents")]
        public async Task<IActionResult> CreateActivityWithDocuments([FromForm] string activityDtoJson)
        {
            if (string.IsNullOrEmpty(activityDtoJson))
                return BadRequest("Activity data is missing.");

            var activityDto = JsonConvert.DeserializeObject<ModuleActivityCreateDto>(activityDtoJson);
            if (activityDto == null)
                return BadRequest("Invalid activity data.");

            if (!await _serviceManager.DateValidationService.ValidateModuleActivityUppdateDatesAsync(activityDto.StartDate, activityDto.EndDate, activityDto.ModuleId))
            {
                ModelState.AddModelError("DateValidation", "Aktiviteter får inte överlappa och starttid måste vara innan sluttid.");
                return BadRequest(ModelState);
            }

            // Read all files from the form dynamically
            var files = Request.Form.Files.ToList(); // <-- grab them all

            var createdActivity = await _serviceManager.ModuleActivityService
                .CreateActivityWithDocumentsAsync(activityDto, files);

            return CreatedAtAction(nameof(GetActivityById), new { id = createdActivity.Id }, createdActivity);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateActivity(Guid id, [FromBody] ModuleActivityUpdateDto activityDto)
        {
            if (activityDto == null || id != activityDto.Id)
                return BadRequest("Activity data is invalid");
            if (!await _serviceManager.DateValidationService.ValidateModuleActivityUppdateDatesAsync(activityDto.StartDate, activityDto.EndDate, activityDto.ModuleId, activityDto.Id))
            {
                ModelState.AddModelError("DateValidation", "End date must be greater than start date and within module dates.");
                return BadRequest(ModelState);
            }
                var updatedActivity = await _serviceManager.ModuleActivityService.UpdateActivityAsync(activityDto);
            if (updatedActivity == null)
                return NotFound();

            return Ok(updatedActivity);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActivity(Guid id)
        {
            var deleted = await _serviceManager.ModuleActivityService.DeleteActivityAsync(id);
            if (!deleted)
                return NotFound();
            return NoContent();
        }
    }
}
