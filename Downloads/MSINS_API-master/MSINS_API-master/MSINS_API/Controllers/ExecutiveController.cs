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

    public class ExecutiveController : ControllerBase
    {
        private readonly IExecutiveService _executiveService;

        public ExecutiveController(IExecutiveService executiveService)
        {
            _executiveService = executiveService;
        }

        /// <summary>
        /// Adds a new executive or updates an existing one.
        /// </summary>
        /// <param name="executiveDto">The executive details to add or update.</param>
        /// <remarks>
        /// <para><b>Functionality:</b></para>
        /// <para>- If <b>ExecutiveId</b> is provided, the existing executive record is updated.</para>
        /// <para>- If <b>ExecutiveId</b> is null, a new executive record is created.</para>
        ///
        /// <para><b>Required Fields:</b></para>
        /// <para>- If <c>ImageFile</c> is provided, it must be a JPG or PNG image of exactly 150*150 pixels and not exceed 500 KB.</para>
        /// <para>- <b>IsActive:</b> Status of the executive (<c>true</c> for active, <c>false</c> for inactive).</para>
        /// <para>- <b>ExecutiveDesc:</b> Description of the executive.</para>
        /// <para>- <b>ExecutiveName:</b> Name of the executive.Max character allowed 255.</para>
        ///
        /// <para><b>Response:</b></para>
        /// <para>- Returns appropriate status codes and messages based on success or failure.</para>
        /// <para>- On success, returns a 200 status with a success message.</para>
        /// <para>- On failure, returns an appropriate error status with a descriptive message.</para>
        /// </remarks>
        /// <returns>Status message indicating success or failure.</returns>
        /// <response code="200">Executive added/updated successfully.</response>
        /// <response code="400">Invalid request due to missing or incorrect parameters.</response>
        /// <response code="500">An unexpected server error occurred.</response>
        [HttpPost("save-executive")]
        public async Task<IActionResult> AddOrUpdateExecutive([FromForm] ExecutiveRequest executiveDto)
        {
            var (statusCode, message) = await _executiveService.AddOrUpdateExecutiveAsync(executiveDto);
            return StatusCode(statusCode, new { Message = message });
        }

        /// <summary>
        /// Retrieves a paginated list of executives with optional filtering.
        /// </summary>
        /// <param name="queryParams">An object containing filters and pagination details.</param>
        /// <remarks>
        /// <para><b>Pagination:</b></para>
        /// <para>- <b>PageSize:</b> The number of records per page (default: <c>10</c>). Must be between <c>1</c> and <c>20</c>.</para>
        /// <para>- <b>PageIndex:</b> The page number for pagination (default: <c>1</c>). Must be greater than <c>0</c>.</para>
        ///
        /// <para><b>Filtering Options:</b></para>
        /// <para>- <b>Search:</b> Perform a search using a partial match in both description and name.</para>
        /// <para>- <b>IsActive:</b> Filter by executive status (<c>true</c> for active, <c>false</c> for inactive).</para>
        /// </remarks>
        /// <returns>A paginated list of executives with a total record count.</returns>
        /// <response code="200">Successfully retrieved executives. If no records found, returns an empty list with message: <c>"No records found."</c></response>
        /// <response code="400">Invalid request due to incorrect pagination or filter parameters.</response>
        /// <response code="500">An unexpected server error occurred.</response>

        [HttpGet("get-executives")]
        public async Task<IActionResult> GetExecutives([FromQuery] ExecutiveQueryParamsRequest queryParams)
        {
            var result = await _executiveService.GetExecutiveAsync(queryParams);

            if (result.Data.Count == 0)
                return Ok(new { message = "No records found.", data = new List<object>() });

            return Ok(result);
        }

    }
}
