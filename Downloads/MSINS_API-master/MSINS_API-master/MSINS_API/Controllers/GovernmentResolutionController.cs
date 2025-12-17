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
    public class GovernmentResolutionController : ControllerBase
    {
        private readonly IGovernmentResolutionService _GovernmentResolutionService;

        public GovernmentResolutionController(IGovernmentResolutionService GovernmentResolutionService)
        {
            _GovernmentResolutionService = GovernmentResolutionService;
        }

        /// <summary>
        /// Adds or updates a featured resource.
        /// </summary>
        /// <param name="GovernmentResolutionDto">The featured resource details to be added or updated.</param>
        /// <remarks>
        /// <para>- If <c>PDFFile</c> is provided, it must be a PDF file and not exceed 100 KB.</para>
        ///
        /// <para><b>IsActive:</b> Determines if the resource is active.</para>
        /// <para>- <c>true</c> → Active</para>
        /// <para>- <c>false</c> → Inactive</para>
        ///
        /// <para><b>Title:</b> The name or title of the featured resource.</para>
        /// <para><b>GovernmentResolutionDate:</b> The date associated with the resource.</para>
        /// </remarks>
        /// <returns>
        /// Returns an HTTP status code and message indicating success or failure.
        /// </returns>
        /// <response code="200">Featured resource added or updated successfully.</response>
        /// <response code="400">Invalid input, validation failed.</response>
        /// <response code="500">Unexpected server error.</response>

        [HttpPost("save-featured-resource")]
        public async Task<IActionResult> AddOrUpdateGovernmentResolution([FromForm] GovernmentResolutionRequest GovernmentResolutionDto)
        {
            var (statusCode, message) = await _GovernmentResolutionService.AddOrUpdateGovernmentResolutionAsync(GovernmentResolutionDto);
            return StatusCode(statusCode, new { Message = message });
        }

        /// <summary>
        /// Retrieves a paginated list of featured resources with optional filtering.
        /// </summary>
        /// <param name="queryParams">An object containing filters and pagination details.</param>
        /// <remarks>
        /// <para><b>Pagination:</b></para>
        /// <para>- <b>PageSize:</b> The number of records per page (default: <c>10</c>). Must be between <c>1</c> and <c>20</c>.</para>
        /// <para>- <b>PageIndex:</b> The page number for pagination (default: <c>1</c>). Must be greater than <c>0</c>.</para>
        ///
        /// <para><b>Filtering Options:</b></para>
        /// <para>- <b>Title:</b> Perform a search using a partial match.</para>
        /// <para>- <b>GovernmentResolutionDate:</b> Filter by a specific date.</para>
        /// <para>- <b>IsActive:</b> Filter by status (<c>true</c> for active, <c>false</c> for inactive).</para>
        /// </remarks>
        /// <returns>A paginated list of featured resources with a total record count.</returns>
        /// <response code="200">Successfully retrieved featured resources. If no records found, returns an empty list with message: <c>"No records found."</c></response>
        /// <response code="400">Invalid request due to incorrect pagination or filter parameters.</response>
        /// <response code="500">An unexpected server error occurred.</response>

        [HttpGet("get-featured-resource")]
        public async Task<IActionResult> GetGovernmentResolutions([FromQuery] GovernmentResolutionQueryParamsRequest queryParams)
        {
            var result = await _GovernmentResolutionService.GetGovernmentResolutionAsync(queryParams);

            if (result.Data.Count == 0)
                return Ok(new { message = "No records found.", data = new List<object>() });

            return Ok(result);
        }
    }
}
