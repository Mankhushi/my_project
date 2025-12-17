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
    [Authorize(Policy = "AdminPolicy")] // globally apply controller

    public class LinkedInController : ControllerBase
    {
        private readonly ILinkedInService _linkedInService;

        public LinkedInController(ILinkedInService linkedInService)
        {
            _linkedInService = linkedInService;
        }

        /// <summary>
        /// Adds or updates a LinkedIn record.
        /// </summary>
        /// <remarks>
        /// This endpoint allows for the creation or modification of a LinkedIn record.
        /// It ensures data validation and handles any errors that may occur during the process.
        /// </remarks>
        /// <returns>
        /// Returns an HTTP status code along with a success or failure message.
        /// </returns>
        /// <response code="200">LinkedIn record added or updated successfully.</response>
        /// <response code="400">Invalid input, validation failed.</response>
        /// <response code="500">Unexpected server error.</response>

        [HttpPost("save-linkedin")]
        public async Task<IActionResult> AddOrUpdateLinkedIn([FromForm] LinkedInRequest linkedInDto)
        {
            var (statusCode, message) = await _linkedInService.AddOrUpdateLinkedInAsync(linkedInDto);

            return StatusCode(statusCode, new { Message = message });
        }

        /// <summary>
        /// Retrieves a paginated list of LinkedIn records with optional filtering.
        /// </summary>
        /// <param name="queryParams">An object containing filters and pagination details.</param>        /// <remarks>
        /// <para><b>Pagination:</b></para>
        /// <para>- <b>PageSize:</b> The number of records per page (default: <c>10</c>). Must be between <c>1</c> and <c>20</c>.</para>
        /// <para>- <b>PageIndex:</b> The page number for pagination (default: <c>1</c>). Must be greater than <c>0</c>.</para>
        /// <para>- <b>IsActive:</b> Filter by banner status (<c>true</c> for active, <c>false</c> for inactive).</para>
        /// </remarks>
        /// <returns>A paginated list of LinkedIn records with a total record count.</returns>
        /// <response code="200">Successfully retrieved LinkedIn records. If no records found, returns an empty list with message: <c>"No records found."</c></response>
        /// <response code="400">Invalid request due to incorrect pagination or filter parameters.</response>
        /// <response code="500">An unexpected server error occurred.</response>
        [HttpGet("get-linkedins")]
        public async Task<IActionResult> GetLinkedIns([FromQuery] LinkedInQueryParamsRequest queryParams)
        {
            var result = await _linkedInService.GetLinkedInsAsync(queryParams);

            if (result.Data.Count == 0)
                return Ok(new { message = "No records found.", data = new List<object>() });

            return Ok(result);
        }
    }
}
