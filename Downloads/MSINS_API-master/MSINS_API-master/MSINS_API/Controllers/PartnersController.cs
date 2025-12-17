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

    public class PartnersController : ControllerBase
    {
        private readonly IPartnerService _partnerService;

        public PartnersController(IPartnerService partnerService)
        {
            _partnerService = partnerService;
        }

        /// <summary>
        /// Adds or updates a partner.
        /// </summary>
        /// <param name="partnerDto">The partner details to be added or updated.</param>
        /// <remarks>
        /// <para><b>Image Requirements:</b></para>
        /// <para>- If <c>ImageFile</c> is provided, it must be a JPG or PNG image of exactly 378x149 pixels and not exceed 100 KB.</para>
        ///
        /// <para><b>LinkType:</b> Determines the type of link.</para>
        /// <para>- <c>true</c> → Internal Link</para>
        /// <para>- <c>false</c> → External Link</para>
        /// 
        /// <para><b><c>PartnerName</c>:</b> Must be unique across all partners. Duplicate names are not allowed.</para>
        ///
        /// </remarks>
        /// <returns>
        /// Returns an HTTP status code and message indicating success or failure.
        /// </returns>
        /// <response code="200">Banner added or updated successfully.</response>
        /// <response code="400">Invalid input, validation failed.</response>
        /// <response code="500">Unexpected server error.</response>

        [HttpPost("save-partner")]
        public async Task<IActionResult> AddOrUpdatePartner([FromForm] PartnersRequest partnerDto)
        {
            var (statusCode, message) = await _partnerService.AddOrUpdatePartnerAsync(partnerDto);

            return StatusCode(statusCode, new { Message = message });
        }

        /// <summary>
        /// Retrieves a paginated list of partners with optional filtering.
        /// </summary>
        /// <param name="queryParams">An object containing search filters and pagination details.</param>
        /// <remarks>
        /// <para><b>Filtering Options:</b></para>
        /// <para>- <b>PartnerName:</b> Supports partial match searching.</para>
        /// <para>- <b>IsActive:</b> Filter by partner status (<c>true</c> for active, <c>false</c> for inactive, <c>null</c> for both).</para>
        ///
        /// <para><b>Pagination:</b></para>
        /// <para>- <b>PageIndex:</b> The page number for pagination (default: <c>1</c>). Must be greater than <c>0</c>.</para>
        /// <para>- <b>PageSize:</b> The number of records per page (default: <c>10</c>). Must be between <c>1</c> and <c>100</c>.</para>
        ///
        /// </remarks>
        /// <returns>A paginated list of partners with a total record count.</returns>
        /// <response code="200">Successfully retrieved partners. If no records found, returns an empty list with message: <c>"No records found."</c></response>
        /// <response code="400">Invalid request due to incorrect pagination or filter parameters.</response>
        /// <response code="500">An unexpected server error occurred.</response>
        [HttpGet("get-partners")]
        public async Task<IActionResult> GetPartners([FromQuery] PartnersQueryParamsRequest queryParams)
        {
            var result = await _partnerService.GetPartnersAsync(queryParams);

            if (result.Data.Count == 0)
                return Ok(new { message = "No records found.", data = new List<object>() });

            return Ok(result);
        }

    }
}
