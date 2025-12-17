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
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }

        /// <summary>
        /// Adds or updates an event.
        /// </summary>
        /// <param name="eventDto">The event details to be added or updated.</param>
        /// <remarks>
        /// <para><b>Image Requirements:</b></para>
        /// <para>- If <c>ImageFile</c> is provided, it must be a JPG, PNG, or WEBP image of exactly 435x246 pixels and not exceed 1000 KB.</para>
        ///
        /// <para><b>Event Type:</b> Specifies the event status.</para>
        /// <para>- <c>Ongoing</c> → Currently happening</para>
        /// <para>- <c>Past</c> → Already completed</para>
        /// <para>- <c>Upcoming</c> → Scheduled for the future</para>
        /// </remarks>
        /// <returns>Returns an HTTP status code and message indicating success or failure.</returns>
        /// <response code="200">Event added or updated successfully.</response>
        /// <response code="400">Invalid input, validation failed.</response>
        /// <response code="500">Unexpected server error.</response>

        [HttpPost("save-event")]
        public async Task<IActionResult> AddOrUpdateEvent([FromForm] EventRequest eventDto)
        {
            var (statusCode, message) = await _eventService.AddOrUpdateEventAsync(eventDto);
            return StatusCode(statusCode, new { Message = message });
        }

        /// <summary>
        /// Retrieves a paginated list of events with optional filtering.
        /// </summary>
        /// <param name="queryParams">An object containing filters and pagination details.</param>
        /// <remarks>
        /// <para><b>Pagination:</b></para>
        /// <para>- <b>PageSize:</b> The number of records per page (default: <c>10</c>). Must be between <c>1</c> and <c>20</c>.</para>
        /// <para>- <b>PageIndex:</b> The page number for pagination (default: <c>1</c>). Must be greater than <c>0</c>.</para>
        ///
        /// <para><b>Filtering Options:</b></para>
        /// <para>- <b>EventType:</b> Filter results by event type (e.g., <c>Ongoing</c>, <c>Past</c>, <c>Upcoming</c>).</para>
        /// <para>- <b>Search:</b> Perform a search using a partial match on event name or description.</para>
        /// <para>- <b>IsActive:</b> Filter by event status (<c>true</c> for active, <c>false</c> for inactive).</para>
        /// <para>- <b>StartDate & EndDate:</b> Filter events within a specific date range.</para>
        /// </remarks>
        /// <returns>A paginated list of events with a total record count.</returns>
        /// <response code="200">Successfully retrieved events. If no records found, returns an empty list with message: <c>"No records found."</c></response>
        /// <response code="400">Invalid request due to incorrect pagination or filter parameters.</response>
        /// <response code="500">An unexpected server error occurred.</response>

        [HttpGet("get-events")]
        public async Task<IActionResult> GetEvents([FromQuery] EventQueryParamRequest queryParams)
        {
            var result = await _eventService.GetEventsAsync(queryParams);

            if (result.Data.Count == 0)
                return Ok(new { message = "No records found.", data = new List<object>() });

            return Ok(result);
        }
    }
}