using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MSINS_API.Models.Request;
using MSINS_API.Services.Interface;

namespace MSINS_API.Controllers
{
    [ApiVersion("1.0")]
    [Route("msins/v{version:apiVersion}/[controller]")] // Versioned route
    [ApiExplorerSettings(GroupName = "v1")]
    [ApiController]
    public class GrievanceController : ControllerBase
    {
        private readonly IGrievanceService _grievanceService;

        public GrievanceController(IGrievanceService grievanceService)
        {
            _grievanceService = grievanceService;
        }

        [HttpPost("send-Grievance")]
        [Authorize(Policy = "AllPolicy")]
        public async Task<IActionResult> SendGrievance([FromBody] GrievanceRequestModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Invalid input data." });
            }

            // Call the service to process the Grievance
            var (statusCode, message) = await _grievanceService.ProcessGrievanceAsync(request);

            // Return the status code and message
            return StatusCode((int)statusCode, new { message });
        }
    }
}
