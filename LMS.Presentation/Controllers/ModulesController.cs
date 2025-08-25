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
    [Route("/modules")]
    [ApiController]
    public class ModulesController: ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        public ModulesController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }
        [HttpGet]
        public async Task<IActionResult> GetModules()
        {
            var modules = await _serviceManager.ModuleService.GetAllModulesAsync();
            return Ok(modules);
        }
        [HttpGet]
        [Route("course/{courseId}")]
        public async Task<IActionResult> GetModulesByCourseId(Guid courseId)
        {
            var modules = await _serviceManager.ModuleService.GetModulesByCourseIdAsync(courseId);
            return Ok(modules);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetModuleById(Guid id)
        {
            var module = await _serviceManager.ModuleService.GetModuleByIdAsync(id);
            if (module == null)
                return NotFound();
            return Ok(module);
        }
        [HttpPost]
        public async Task<IActionResult> CreateModule([FromBody] ModuleCreateDto moduleDto)
        {
            if (moduleDto == null)
                return BadRequest("Module data is null");
            var createdModule = await _serviceManager.ModuleService.CreateModuleAsync(moduleDto);
            return CreatedAtAction(nameof(GetModuleById), new { id = createdModule.Id }, createdModule);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateModule(Guid id, [FromBody] ModuleUpdateDto moduleUpdDto)
        {
            if (moduleUpdDto == null || id != moduleUpdDto.Id)
                return BadRequest("Module data is invalid");
            var updatedModule = await _serviceManager.ModuleService.UpdateModuleAsync(moduleUpdDto);
            if (updatedModule == null)
                return NotFound();
            return Ok(updatedModule);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteModule(Guid id)
        {
            var deleted = await _serviceManager.ModuleService.DeleteModuleAsync(id);
            if (!deleted)
                return NotFound();
            return NoContent();
        }
    }
}
