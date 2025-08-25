using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Presentation.Controllers
{
    [Route("/activity-types")]
    [ApiController]
    public class ActivityTypesController: ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        public ActivityTypesController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }
        [HttpGet]
        public async Task<IActionResult> GetActivityTypes()
        {
            var types =  await _serviceManager.ActivityTypeService.GetAllActivityTypesAsync();
            return Ok(types);
        }
    }
}
