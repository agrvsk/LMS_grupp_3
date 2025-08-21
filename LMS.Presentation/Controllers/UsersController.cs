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
    [Route("/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        public UsersController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _serviceManager.UserService.GetAllUsersAsync();
            return Ok(users);
        }
        [HttpGet("course/{courseId}")]
        public async Task<IActionResult> GetUsersByCourseId(Guid courseId)
        {
            var users = await _serviceManager.UserService.GetUsersByCourseIdAsync(courseId);
            return Ok(users);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _serviceManager.UserService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound();
            return Ok(user);
        }
    }
}
