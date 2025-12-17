using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MSINS_API.Models.Request;
using MSINS_API.Services.Interface;
using System.ComponentModel.DataAnnotations;

namespace MSINS_API.Controllers
{
    [ApiVersion("1.0")]
    [Route("msins/v{version:apiVersion}/[controller]")]
    [ApiExplorerSettings(GroupName = "v1")]
    [ApiController]
    [Authorize(Policy = "AdminPolicy")]
    public class NewSpeakersMasterController : ControllerBase
    {
        private readonly INewSpeakersMasterService _speakerService;

        public NewSpeakersMasterController(INewSpeakersMasterService speakerService)
        {
            _speakerService = speakerService;
        }

        //  ADD SPEAKER ----------------->>>>

        /// <summary>
        /// Adds a new speaker to the system.
        /// </summary>
        /// <param name="model">The speaker details to be added.</param>
        /// <remarks>
        /// <para><b>Image Requirements:</b></para>
        /// <para>- File types allowed: JPG, JPEG, PNG, WEBP</para>
        /// <para>- Maximum size: 500 KB</para>
        /// <para>- Recommended size: 300x300 pixels</para>
        ///
        /// <para><b>Required Fields:</b></para>
        /// <para>- <c>SpeakerName</c>: Speaker’s full name (must be unique)</para>
        /// <para>- <c>Designation</c>: Professional title</para>
        /// <para>- <c>Organization</c>: Associated organization</para>
        /// </remarks>
        /// <returns>Returns an HTTP status code and a success/failure message.</returns>
        /// <response code="200">Speaker added successfully.</response>
        /// <response code="400">Validation or file upload failed.</response>
        /// <response code="500">An unexpected server error occurred.</response>
        [HttpPost("add-speaker")]
        public async Task<IActionResult> AddSpeaker([FromForm] NewSpeakersMasterRequest model)
        {
            // Validate the incoming model manually
            var context = new ValidationContext(model, null, null);
            var results = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(model, context, results, true);

            if (!isValid)
            {
                return BadRequest(new
                {
                    message = "Validation failed.",
                    errors = results.Select(e => e.ErrorMessage)
                });
            }

            var (statusCode, message) = await _speakerService.AddSpeakerAsync(model);
            return StatusCode(statusCode, new { Message = message });
        }

        // UPDATE SPEAKER -------------------------------------------->>>

        /// <summary>
        /// Updates an existing speaker’s details.
        /// </summary>
        /// <param name="id">Speaker ID to be updated.</param>
        /// <param name="model">The updated speaker details.</param>
        /// <remarks>
        /// <para><b>Notes:</b></para>
        /// <para>- You may optionally upload a new image; existing image will remain if not provided.</para>
        /// <para>- <c>SpeakerName</c> must remain unique across all records.</para>
        /// </remarks>
        /// <returns>Returns an HTTP status code and message indicating update success or failure.</returns>
        /// <response code="200">Speaker updated successfully.</response>
        /// <response code="400">Invalid ID or input data.</response>
        /// <response code="404">Speaker not found.</response>
        /// <response code="500">Unexpected server error.</response>
        /// 

        [HttpPut("update-speaker/{id}")]
        public async Task<IActionResult> UpdateSpeaker(int id, [FromForm] NewSpeakersMasterRequest model)
        {
            if (id <= 0)
                return BadRequest(new { Message = "Invalid Speaker ID." });

            var (statusCode, message) = await _speakerService.UpdateSpeakerAsync(id, model);
            return StatusCode(statusCode, new { Message = message });
        }

        //  GET SPEAKER BY ID ------------------------------------>>>>>

        /// <summary>
        /// Retrieves a specific speaker by their unique ID.
        /// </summary>
        /// <param name="id">The unique ID of the speaker.</param>
        /// <returns>Speaker details if found.</returns>
        /// <response code="200">Speaker record found successfully.</response>
        /// <response code="400">Invalid ID supplied.</response>
        /// <response code="404">Speaker not found.</response>
        [HttpGet("get-speaker/{id}")]
        public async Task<IActionResult> GetSpeakerById(int id)
        {
            if (id <= 0)
                return BadRequest(new { Message = "Invalid Speaker ID." });

            var (statusCode, message, data) = await _speakerService.GetSpeakerByIdAsync(id);

            if (data == null)
                return StatusCode(statusCode, new { Message = message });

            return StatusCode(statusCode, new { Message = message, Data = data });
        }

        //  GET ALL SPEAKERS (Paginated)  --------------------------------------->>>

        /// <summary>
        /// Retrieves a paginated list of all speakers.
        /// </summary>
        /// <param name="pageNumber">The page number to fetch (default: 1).</param>
        /// <param name="pageSize">The number of records per page (default: 10).</param>
        /// <returns>A paginated list of speakers with total count.</returns>
        /// <response code="200">Speakers retrieved successfully.</response>
        /// <response code="404">No speakers found.</response>
        /// <response code="500">Unexpected server error.</response>
        [HttpGet("get-speakers")]
        public async Task<IActionResult> GetAllSpeakers(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchTerm = null)
        {
            var result = await _speakerService.GetAllSpeakersAsync(pageNumber, pageSize, searchTerm);

            if (result.Data.Count == 0)
                return Ok(new { message = "No speakers found.", data = new List<object>() });

            return Ok(result);
        }

    }
}
                                                         