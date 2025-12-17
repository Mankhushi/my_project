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

    public class VideoController : ControllerBase
    {
        private readonly IVideoService _videoService;

        public VideoController(IVideoService VideoService)
        {
            _videoService = VideoService;
        }

        /// <summary>
        /// Adds or updates a video.
        /// </summary>
        /// <param name="videoDto">The video details to be added or updated.</param>
        /// <remarks>
        /// <para><b>Video Requirements:</b></para>
        /// <para>- <c>VideoId</c> pass null for adding a new video.</para>
        /// <para>- <c>Link</c> must be a valid URL.</para>
        /// <para>- <c>Title</c> should not exceed 255 characters.</para>
        ///
        /// <para><b>Video Flags:</b></para>
        /// <para>- <c>IsFeatured</c> → Marks the video as featured.</para>
        /// <para>- <c>IsLatest</c> → Marks the video as latest.</para>
        /// <para>- <c>IsActive</c> → Determines whether the video is active.</para>
        /// </remarks>
        /// <returns>
        /// Returns an HTTP status code and message indicating success or failure.
        /// </returns>
        /// <response code="200">Video added or updated successfully.</response>
        /// <response code="400">Invalid input, validation failed.</response>
        /// <response code="500">Unexpected server error.</response>

        [HttpPost("save-video")]
        public async Task<IActionResult> AddOrUpdateVideo([FromBody] VideoRequest videoDto)
        {
            var (statusCode, message) = await _videoService.AddOrUpdateVideoAsync(videoDto);

            return StatusCode(statusCode, new { Message = message });
        }

        /// <summary>
        /// Retrieves a paginated list of videos with optional filtering.
        /// </summary>
        /// <param name="queryParams">An object containing filters and pagination details.</param>
        /// <remarks>
        /// <para><b>Pagination:</b></para>
        /// <para>- <b>PageSize:</b> The number of records per page (default: <c>10</c>). Must be between <c>1</c> and <c>20</c>.</para>
        /// <para>- <b>PageIndex:</b> The page number for pagination (default: <c>1</c>). Must be greater than <c>0</c>.</para>
        ///
        /// <para><b>Filtering Options:</b></para>
        /// <para>- <b>SearchTerm:</b> Perform a search using a partial match on title or link.</para>
        /// <para>- <b>IsActive:</b> Filter by video status (<c>true</c> for active, <c>false</c> for inactive).</para>
        /// <para>- <b>FilterType:</b> Filter by video type:</para>
        /// <para>   - <c>Featured</c>: Returns only featured videos.</para>
        /// <para>   - <c>Latest</c>: Returns only latest videos.</para>
        /// <para>   - <c>All</c>: Returns all videos (default behavior).</para>
        /// </remarks>
        /// <returns>A paginated list of videos with a total record count.</returns>
        /// <response code="200">Successfully retrieved videos. If no records found, returns an empty list with message: <c>"No records found."</c></response>
        /// <response code="400">Invalid request due to incorrect pagination or filter parameters.</response>
        /// <response code="500">An unexpected server error occurred.</response>

        [HttpGet("get-videos")]
        public async Task<IActionResult> GetVideos([FromQuery] VideoQueryParamsRequest queryParams)
        {
            var result = await _videoService.GetVideosAsync(queryParams);

            if (result.Data.Count == 0)
                return Ok(new { message = "No records found.", data = new List<object>() });

            return Ok(result);
        }

    }
}
