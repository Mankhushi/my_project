using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MSINS_API.Models.Request;
using MSINS_API.Services.Implementation;
using MSINS_API.Services.Interface;
using System.ComponentModel.DataAnnotations;

namespace MSINS_API.Controllers
{
    [ApiVersion("1.0")]
    [Route("msins/v{version:apiVersion}/[controller]")] // Versioned route
    [ApiExplorerSettings(GroupName = "v1")]
    [ApiController]
    [Authorize(Policy = "AdminPolicy")] // globally apply controller

    public class EcoSystemController : ControllerBase
    {

        private readonly IEcoSystemService _ecosystemService;

        public EcoSystemController(IEcoSystemService ecosystemService)
        {
            _ecosystemService = ecosystemService;
        }

        /// <summary>
        /// Adds or updates an ecosystem entry.
        /// </summary>
        /// <param name="ecoSystemDto">The ecosystem details to be added or updated.</param>
        /// <remarks>
        /// <para><b>Image Requirements:</b></para>
        /// <para>- If <c>ImageFile</c> is provided, it must be a JPG or PNG image of exactly 154*154 pixels and not exceed 50 KB.</para>
        ///
        /// <para><b>Active Status:</b> Determines if the ecosystem entry is active.</para>
        /// <para>- <c>true</c> → Active</para>
        /// <para>- <c>false</c> → Inactive</para>
        ///
        /// <para><b>Text Fields:</b></para>
        /// <para>- <c>TextOne</c>: Primary text information related to the ecosystem.</para>
        /// <para>- <c>TextTwo</c>: Secondary text information.</para>
        /// </remarks>
        /// <returns>
        /// Returns an HTTP status code and message indicating success or failure.
        /// </returns>
        /// <response code="200">Ecosystem entry added or updated successfully.</response>
        /// <response code="400">Invalid input, validation failed.</response>
        /// <response code="500">Unexpected server error.</response>


        [HttpPost("save-ecosystem")]
        public async Task<IActionResult> AddOrUpdateEcoSystem([FromForm] EcoSystemRequest ecosystemDto)
        {
            // Manually trigger validation for IValidatableObject
            var context = new ValidationContext(ecosystemDto, null, null);
            var results = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(ecosystemDto, context, results, true);

            if (!isValid)
            {
                return BadRequest(new
                {
                    message = "Validation failed.",
                    errors = results.Select(e => e.ErrorMessage)
                });
            }

            var (statusCode, message) = await _ecosystemService.AddOrUpdateEcoSystemAsync(ecosystemDto);

            return StatusCode(statusCode, new { Message = message });
        }

        /// <summary>
        /// Retrieves a paginated list of ecosystems with optional filtering.
        /// </summary>
        /// <param name="queryParams">An object containing filters and pagination details.</param>
        /// <remarks>
        /// <para><b>Pagination:</b></para>
        /// <para>- <b>PageSize:</b> The number of records per page (default: <c>10</c>). Must be between <c>1</c> and <c>20</c>.</para>
        /// <para>- <b>PageIndex:</b> The page number for pagination (default: <c>1</c>). Must be greater than <c>0</c>.</para>
        ///
        /// <para><b>Filtering Options:</b></para>
        /// <para>- <b>TextOne:</b> Perform a search using a partial match.</para>
        /// <para>- <b>TextTwo:</b> Perform a search using a partial match.</para>
        /// <para>- <b>IsActive:</b> Filter by ecosystem status (<c>true</c> for active, <c>false</c> for inactive).</para>
        /// </remarks>
        /// <returns>A paginated list of ecosystems with a total record count.</returns>
        /// <response code="200">Successfully retrieved ecosystems. If no records found, returns an empty list with message: <c>"No records found."</c></response>
        /// <response code="400">Invalid request due to incorrect pagination or filter parameters.</response>
        /// <response code="500">An unexpected server error occurred.</response>


        [HttpGet("get-ecosystems")]
        public async Task<IActionResult> GetEcoSystem([FromQuery] EcoSystemQueryParamsRequest queryParams)
        {
            var result = await _ecosystemService.GetEcoSystemAsync(queryParams);

            if (result.Data.Count.Equals(0))
                return Ok(new { message = "No records found.", data = new List<object>() });

            return Ok(result);
        }
    }
}
