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

    public class TenderNotificationController : ControllerBase
    {
        private readonly ITenderNotificationService _tenderService;

        public TenderNotificationController(ITenderNotificationService tenderService)
        {
            _tenderService = tenderService;
        }

        /// <summary>
        /// Adds or updates a Tender Notification.
        /// </summary>
        /// <param name="tenderDto">The details of the Tender Notification to be added or updated.</param>
        /// <remarks>
        /// <para><b>IsActive:</b> Determines the status of the tender.</para>
        /// <para>- <c>true</c> → Active</para>
        /// <para>- <c>false</c> → Inactive</para>
        ///
        /// <para><b>Title:</b> The title of the tender (Max: 255 characters).</para>
        /// <para><b>RefNo:</b> The reference number of the tender (Max: 255 characters).</para>
        /// <para><b>Category:</b> The category of the tender (Max: 255 characters).</para>
        /// <para><b>PublishedDate:</b> The date when the tender was published.</para>
        /// <para><b>Status:</b> Current status of the tender (Max: 255 characters).</para>
        /// <para><b>TenderID:</b> The ID of the tender (Max: 255 characters).</para>
        /// </remarks>
        /// <returns>
        /// Returns an HTTP status code and message indicating success or failure.
        /// </returns>
        /// <response code="200">Tender notification added or updated successfully.</response>
        /// <response code="400">Invalid input, validation failed.</response>
        /// <response code="500">Unexpected server error.</response>

        [HttpPost("save-tender-notification")]
        public async Task<IActionResult> AddOrUpdateTenderNotification([FromForm] TenderNotificationRequest tenderDto)
        {
            var (statusCode, message) = await _tenderService.AddOrUpdateTenderNotificationAsync(tenderDto);
            return StatusCode(statusCode, new { Message = message });
        }

        /// <summary>
        /// Retrieves a paginated list of Tender Notifications with optional filtering.
        /// </summary>
        /// <param name="queryParams">An object containing filters and pagination details.</param>
        /// <remarks>
        /// <para><b>Pagination:</b></para>
        /// <para>- <b>PageSize:</b> The number of records per page (default: <c>10</c>). Must be between <c>1</c> and <c>20</c>.</para>
        /// <para>- <b>PageIndex:</b> The page number for pagination (default: <c>1</c>). Must be greater than <c>0</c>.</para>
        ///
        /// <para><b>Filtering Options:</b></para>
        /// <para>- <b>Search:</b> Perform a partial match search on <c>RefNo</c>, <c>Title</c>, <c>Tender ID</c>, or <c>Category</c>.</para>
        /// <para>- <b>PublishedFrom - PublishedTo:</b> Filter by published date range.</para>
        /// <para>- <b>IsActive:</b> Filter by active status (<c>true</c> for active, <c>false</c> for inactive).</para>
        /// </remarks>
        /// <returns>A paginated list of Tender Notifications with a total record count.</returns>
        /// <response code="200">Successfully retrieved tenders. If no records found, returns an empty list with message: <c>"No records found."</c></response>
        /// <response code="400">Invalid request due to incorrect pagination or filter parameters.</response>
        /// <response code="500">An unexpected server error occurred.</response>

        [HttpGet("get-tender-notifications")]
        public async Task<IActionResult> GetTenderNotifications([FromQuery] TenderNotificationQueryParamsRequest queryParams)
        {
            var result = await _tenderService.GetTenderNotificationAsync(queryParams);

            if (result.Data.Count == 0)
                return Ok(new { message = "No records found.", data = new List<object>() });

            return Ok(result);
        }

    }
}
