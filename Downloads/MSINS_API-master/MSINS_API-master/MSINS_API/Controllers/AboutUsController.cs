using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MSINS_API.Models.Request;
using MSINS_API.Repositories.Interface;
using MSINS_API.Services.Implementation;
using MSINS_API.Services.Interface;

namespace MSINS_API.Controllers
{
    [ApiVersion("1.0")]
    [Route("msins/v{version:apiVersion}/[controller]")] // Versioned route
    [ApiExplorerSettings(GroupName = "v1")]
    [ApiController]
    [Authorize(Policy = "AdminPolicy")] // globally apply controller

    public class AboutUsController : ControllerBase
    {
        private readonly IAboutUsService _aboutUsService;

        public AboutUsController(IAboutUsService aboutUsService)
        {
            _aboutUsService = aboutUsService;
        }

        /// <summary>
        /// Adds or updates the About Us section.
        /// </summary>
        /// <param name="aboutDto">The About Us details to be added or updated.</param>
        /// <remarks>
        /// <para><b>Image Requirements:</b></para>
        /// <para>- If <c>ImageFile</c> is provided, it must be a JPG or PNG image of exactly 2035x2129 pixels and not exceed 500 KB.</para>
        ///
        /// <para><b>PDF Requirements:</b></para>
        /// <para>- If <c>PDFFile</c> is provided, it must be a valid PDF file.</para>
        ///
        /// <para>This API updates the text content and optionally updates the image and PDF associated with the About Us section.</para>
        /// </remarks>
        /// <returns>
        /// Returns an HTTP status code and message indicating success or failure.
        /// </returns>
        /// <response code="200">About Us section updated successfully.</response>
        /// <response code="400">Invalid input, validation failed.</response>
        /// <response code="500">Unexpected server error.</response>

        [HttpPost("save-aboutus")]
        public async Task<IActionResult> AddOrUpdateAboutUs([FromForm] AboutUsRequest aboutDto)
        {
            var (statusCode, message) = await _aboutUsService.AddOrUpdateAboutUsAsync(aboutDto);

            return StatusCode(statusCode, new { Message = message });
        }

        /// <summary>
        /// Retrieves About Us details.
        /// </summary>
        /// <remarks>
        /// <para>Fetches details including text content, image, and PDF file paths.</para>
        /// </remarks>
        /// <returns>Returns the About Us details.</returns>
        /// <response code="200">Returns the About Us details successfully.</response>
        /// <response code="404">If no About Us details are found.</response>
        /// <response code="500">Unexpected server error.</response>
        [HttpGet("get-aboutus")]
        public async Task<IActionResult> GetAboutUs()
        {
            var aboutUsData = await _aboutUsService.GetAboutUsAsync();

            if (aboutUsData == null)
            {
                return Ok(new { message = "No records found.", data = new List<object>() });
            }                

            return Ok(aboutUsData);
        }

    }
}
