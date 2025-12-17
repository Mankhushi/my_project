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
    public class SuggestionController : ControllerBase
    {
        private readonly ISuggestionService _suggestionService;

        public SuggestionController(ISuggestionService suggestionService)
        {
            _suggestionService = suggestionService;
        }

        [HttpPost("send-suggestion")]
        [Authorize(Policy = "AllPolicy")]
        public async Task<IActionResult> SendSuggestion([FromBody] SuggestionRequestModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Invalid input data." });
            }

            // Call the service to process the suggestion
            var (statusCode, message) = await _suggestionService.ProcessSuggestionAsync(request);

            // Return the status code and message
            return StatusCode((int)statusCode, new { message });
        }
    }
}
