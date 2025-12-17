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
    public class MediaController : ControllerBase
    {
        private readonly IMediaService _MediaService;

        public MediaController(IMediaService MediaService)
        {
            _MediaService = MediaService;
        }

        /// <summary>
        /// Adds or updates an Media.
        /// </summary>
        /// <param name="MediaDto">The Media details to be added or updated.</param>
        /// <remarks>
        /// <para><b>Image Requirements:</b></para>
        /// <para>- If <c>ImageFile</c> is provided, it must be a JPG, PNG, or WEBP image of exactly 274*184 pixels and not exceed 1000 KB.</para>
        /// </remarks>
        /// <returns>Returns an HTTP status code and message indicating success or failure.</returns>
        /// <response code="200">Media added or updated successfully.</response>
        /// <response code="400">Invalid input, validation failed.</response>
        /// <response code="500">Unexpected server error.</response>

        [HttpPost("save-Media")]
        public async Task<IActionResult> AddOrUpdateMedia([FromForm] MediaRequest MediaDto)
        {
            var (statusCode, message) = await _MediaService.AddOrUpdateMediaAsync(MediaDto);
            return StatusCode(statusCode, new { Message = message });
        }

        /// <summary>
        /// Retrieves a paginated list of Medias with optional filtering.
        /// </summary>
        /// <param name="queryParams">An object containing filters and pagination details.</param>
        /// <remarks>
        /// <para><b>Pagination:</b></para>
        /// <para>- <b>PageSize:</b> The number of records per page (default: <c>10</c>). Must be between <c>1</c> and <c>20</c>.</para>
        /// <para>- <b>PageIndex:</b> The page number for pagination (default: <c>1</c>). Must be greater than <c>0</c>.</para>
        ///
        /// <para><b>Filtering Options:</b></para>
        /// <para>- <b>Search:</b> Perform a search using a partial match on Media name or description.</para>
        /// <para>- <b>IsActive:</b> Filter by Media status (<c>true</c> for active, <c>false</c> for inactive).</para>
        /// <para>- <b>StartDate & EndDate:</b> Filter Medias within a specific date range.</para>
        /// </remarks>
        /// <returns>A paginated list of Medias with a total record count.</returns>
        /// <response code="200">Successfully retrieved Medias. If no records found, returns an empty list with message: <c>"No records found."</c></response>
        /// <response code="400">Invalid request due to incorrect pagination or filter parameters.</response>
        /// <response code="500">An unexpected server error occurred.</response>

        [HttpGet("get-medias")]
        public async Task<IActionResult> GetMedias([FromQuery] MediaQueryParamRequest queryParams)
        {
            var result = await _MediaService.GetMediasAsync(queryParams);

            if (result.Data.Count == 0)
                return Ok(new { message = "No records found.", data = new List<object>() });

            return Ok(result);
        }
    }
}
