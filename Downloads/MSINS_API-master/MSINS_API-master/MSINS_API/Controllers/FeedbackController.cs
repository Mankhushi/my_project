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
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService _feedbackService;

        public FeedbackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        [HttpPost("send-feedback")]
        [Authorize(Policy = "AllPolicy")]
        public async Task<IActionResult> Sendfeedback([FromBody] FeedbackRequestModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Invalid input data." });
            }

            // Call the service to process the feedback
            var (statusCode, message) = await _feedbackService.ProcessFeedbackAsync(request);

            // Return the status code and message
            return StatusCode((int)statusCode, new { message });
        }
    }
}
