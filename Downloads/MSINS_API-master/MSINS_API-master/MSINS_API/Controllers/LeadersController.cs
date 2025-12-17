using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MSINS_API.Models.Request;
using MSINS_API.Services.Implementation;
using MSINS_API.Services.Interface;

namespace MSINS_API.Controllers
{
    [ApiVersion("1.0")]
    [Route("msins/v{version:apiVersion}/[controller]")] // Versioned route
    [ApiExplorerSettings(GroupName = "v1")]
    [ApiController]
    [Authorize(Policy = "AdminPolicy")] // globally apply controller

    public class LeadersController : ControllerBase
    {
        private readonly ILeadershipService _leaderService;

        public LeadersController(ILeadershipService leaderService)
        {
            _leaderService = leaderService;
        }

        /// <summary>
        /// Adds or updates a leadership entry.
        /// </summary>
        /// <param name="leaderDto">The leadership details to be added or updated.</param>
        /// <remarks>
        /// <para><b>Image Requirements:</b></para>
        /// <para>- If <c>ImageFile</c> is provided, it must be a JPG or PNG image with a recommended resolution of 400x400 pixels and not exceed 200 KB.</para>
        ///
        /// <para><b>Leader Name:</b> Specifies the name of the leader.</para>
        /// 
        /// <para><b>Designation:</b> Specifies the designation of the leader.</para>
        ///
        /// <para><b>IsActive:</b> Determines whether the leader is currently active.</para>
        /// <para>- <c>true</c> → Active Leader</para>
        /// <para>- <c>false</c> → Inactive Leader</para>
        ///
        /// </remarks>
        /// <returns>
        /// Returns an HTTP status code and message indicating success or failure.
        /// </returns>
        /// <response code="200">Leadership entry added or updated successfully.</response>
        /// <response code="400">Invalid input, validation failed.</response>
        /// <response code="500">Unexpected server error.</response>


        [HttpPost("save-leader")]
        public async Task<IActionResult> AddOrUpdateLeadership([FromForm] LeadershipRequest leaderDto)
        {
            var (statusCode, message) = await _leaderService.AddOrUpdateLeadershipAsync(leaderDto);

            return StatusCode(statusCode, new { Message = message });
        }

        /// <summary>
        /// Retrieves a paginated list of leadership members with optional filtering.
        /// </summary>
        /// <param name="queryParams">An object containing filters and pagination details.</param>
        /// <remarks>
        /// <para><b>Pagination:</b></para>
        /// <para>- <b>PageSize:</b> The number of records per page (default: <c>10</c>). Must be between <c>1</c> and <c>20</c>.</para>
        /// <para>- <b>PageIndex:</b> The page number for pagination (default: <c>1</c>). Must be greater than <c>0</c>.</para>
        ///
        /// <para><b>Filtering Options:</b></para>
        /// <para>- <b>LeaderName:</b> Perform a search using a partial match.</para>
        /// <para>- <b>Designation:</b> Filter results by designation.</para>
        /// <para>- <b>IsActive:</b> Filter by status (<c>true</c> for active, <c>false</c> for inactive).</para>
        /// </remarks>
        /// <returns>A paginated list of leadership members with a total record count.</returns>
        /// <response code="200">Successfully retrieved leadership members. If no records found, returns an empty list with message: <c>"No records found."</c></response>
        /// <response code="400">Invalid request due to incorrect pagination or filter parameters.</response>
        /// <response code="500">An unexpected server error occurred.</response>
        [HttpGet("get-leaders")]
        public async Task<IActionResult> GetLeaders([FromQuery] LeaderQueryParamsRequest queryParams)
        {
            var result = await _leaderService.GetLeadersAsync(queryParams);

            if (result.Data.Count == 0)
                return Ok(new { message = "No records found.", data = new List<object>() });

            return Ok(result);
        }
    }
}
