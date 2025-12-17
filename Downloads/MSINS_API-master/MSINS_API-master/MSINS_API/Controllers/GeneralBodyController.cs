using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MSINS_API.Models.Request;
using MSINS_API.Repositories.Interface;
using MSINS_API.Services.Interface;

namespace MSINS_API.Controllers
{
    [ApiVersion("1.0")]
    [Route("msins/v{version:apiVersion}/[controller]")] // Versioned route
    [ApiExplorerSettings(GroupName = "v1")]
    [ApiController]
    [Authorize(Policy = "AdminPolicy")] // globally apply controller

    public class GeneralBodyController : ControllerBase
    {
        private readonly IGeneralBodyService _generalbodyService;

        public GeneralBodyController(IGeneralBodyService generalbodyService)
        {
            _generalbodyService = generalbodyService;
        }


        /// <summary>
        /// Adds or updates a General Body member.
        /// </summary>
        /// <param name="GeneralBodyDto">The details of the General Body member to be added or updated.</param>
        /// <remarks>
        /// <para><b>IsActive:</b> Determines the status of the member.</para>
        /// <para>- <c>true</c> → Active</para>
        /// <para>- <c>false</c> → Inactive</para>
        ///
        /// <para><b>Name:</b> The full name of the member (Max: 255 characters).</para>
        /// <para><b>Designation:</b> Represents the official role or title of the member (Max: 255 characters).</para>
        /// <para><b>Position:</b> Represents the rank or hierarchical position of the member (Max: 255 characters).</para>
        /// 
        /// </remarks>
        /// <returns>
        /// Returns an HTTP status code and message indicating success or failure.
        /// </returns>
        /// <response code="200">General Body member added or updated successfully.</response>
        /// <response code="400">Invalid input, validation failed.</response>
        /// <response code="500">Unexpected server error.</response>

        [HttpPost("save-general-body")]
        public async Task<IActionResult> AddOrUpdateGeneralBody([FromForm] GeneralBodyRequest GeneralBodyDto)
        {
            var (statusCode, message) = await _generalbodyService.AddOrUpdateGeneralBodyAsync(GeneralBodyDto);
            return StatusCode(statusCode, new { Message = message });
        }

        /// <summary>
        /// Retrieves a paginated list of General Body members with optional filtering.
        /// </summary>
        /// <param name="queryParams">An object containing filters and pagination details.</param>
        /// <remarks>
        /// <para><b>Pagination:</b></para>
        /// <para>- <b>PageSize:</b> The number of records per page (default: <c>10</c>). Must be between <c>1</c> and <c>20</c>.</para>
        /// <para>- <b>PageIndex:</b> The page number for pagination (default: <c>1</c>). Must be greater than <c>0</c>.</para>
        ///
        /// <para><b>Filtering Options:</b></para>
        /// <para>- <b>Search:</b> Perform a partial match search on <c>Name</c>, <c>Designation</c>, <c>Adderss</c> or <c>Position</c>.</para>
        /// <para>- <b>IsActive:</b> Filter by active status (<c>true</c> for active, <c>false</c> for inactive).</para>
        /// </remarks>
        /// <returns>A paginated list of General Body members with a total record count.</returns>
        /// <response code="200">Successfully retrieved members. If no records found, returns an empty list with message: <c>"No records found."</c></response>
        /// <response code="400">Invalid request due to incorrect pagination or filter parameters.</response>
        /// <response code="500">An unexpected server error occurred.</response>

        [HttpGet("get-general-body")]
        public async Task<IActionResult> GetGeneralBody([FromQuery] GeneralBodyQueryParamRequest queryParams)
        {
            var result = await _generalbodyService.GetGeneralBodyAsync(queryParams);

            if (result.Data.Count == 0)
                return Ok(new { message = "No records found.", data = new List<object>() });

            return Ok(result);
        }

    }
}
