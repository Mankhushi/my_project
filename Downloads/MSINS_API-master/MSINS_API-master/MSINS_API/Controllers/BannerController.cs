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

    public class BannerController : ControllerBase
    {
        private readonly IBannerService _bannerService;

        public BannerController(IBannerService Bannerservice)
        {
            _bannerService = Bannerservice;
        }

        /// <summary>
        /// Adds or updates a banner.
        /// </summary>
        /// <param name="bannerDto">The banner details to be added or updated.</param>
        /// <remarks>
        /// <para><b>Image Requirements:</b></para>
        /// <para>- If <c>ImageFile</c> is provided, it must be a JPG or PNG image of exactly 3456x1728 pixels and not exceed 1000 KB.</para>
        ///
        /// <para><b>LinkType:</b> Determines the type of link.</para>
        /// <para>- <c>true</c> → Internal Link</para>
        /// <para>- <c>false</c> → External Link</para>
        ///
        /// <para><b>BannerType:</b> Determines the type of banner.</para>
        /// <para>- <c>Home</c> → Home Page banner</para>
        /// <para>- <c>Resources</c> → Resources Page banner</para>
        /// </remarks>
        /// <returns>
        /// Returns an HTTP status code and message indicating success or failure.
        /// </returns>
        /// <response code="200">Banner added or updated successfully.</response>
        /// <response code="400">Invalid input, validation failed.</response>
        /// <response code="500">Unexpected server error.</response>

        [HttpPost("save-banner")]
        public async Task<IActionResult> AddOrUpdateBanner([FromForm] BannerRequest bannerDto)
        {
            var (statusCode, message) = await _bannerService.AddOrUpdateBannerAsync(bannerDto);

            return StatusCode(statusCode, new { Message = message });
        }

        /// <summary>
        /// Retrieves a paginated list of banners with optional filtering.
        /// </summary>
        /// <param name="queryParams">An object containing filters and pagination details.</param>
        /// <remarks>
        /// <para><b>Pagination:</b></para>
        /// <para>- <b>PageSize:</b> The number of records per page (default: <c>10</c>). Must be between <c>1</c> and <c>20</c>.</para>
        /// <para>- <b>PageIndex:</b> The page number for pagination (default: <c>1</c>). Must be greater than <c>0</c>.</para>
        ///
        /// <para><b>Filtering Options:</b></para>
        /// <para>- <b>BannerType:</b> Filter results by banner type (e.g., <c>Home</c>, <c>Resources</c>).</para>
        /// <para>- <b>BannerName:</b> Perform a search using a partial match.</para>
        /// <para>- <b>IsActive:</b> Filter by banner status (<c>true</c> for active, <c>false</c> for inactive).</para>
        /// </remarks>
        /// <returns>A paginated list of banners with a total record count.</returns>
        /// <response code="200">Successfully retrieved banners. If no records found, returns an empty list with message: <c>"No records found."</c></response>
        /// <response code="400">Invalid request due to incorrect pagination or filter parameters.</response>
        /// <response code="500">An unexpected server error occurred.</response>

        [HttpGet("get-banners")]
        public async Task<IActionResult> GetBanners([FromQuery] BannerQueryParamsRequest queryParams)
        {
            var result = await _bannerService.GetBannersAsync(queryParams);

            if (result.Data.Count.Equals(0))
                return Ok(new { message = "No records found.", data = new List<object>() });

            return Ok(result);
        }
    }
}
