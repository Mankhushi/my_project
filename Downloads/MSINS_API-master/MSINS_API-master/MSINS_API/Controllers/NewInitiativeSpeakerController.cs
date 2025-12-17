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
    public class NewInitiativeSpeakerController : ControllerBase
    {
        private readonly INewInitiativeSpeakerService _speakerService;

        public NewInitiativeSpeakerController(INewInitiativeSpeakerService speakerService)
        {
            _speakerService = speakerService;
        }

        // ---------------------------------------------------------------
        // 🔹 ADD SPEAKER
        // ---------------------------------------------------------------
        /// <summary>
        /// Adds a new initiative speaker.
        /// </summary>
        /// <param name="request">Speaker details to add.</param>
        /// <returns>Status code and message.</returns>
        [HttpPost("add-speaker")]
        public async Task<IActionResult> AddSpeaker([FromForm] NewInitiativeSpeakerRequest request)
        {
            var (code, message) = await _speakerService.AddSpeakerAsync(request);
            return StatusCode(code, new { Message = message });
        }


        // ---------------------------------------------------------------
        // 🔹 UPDATE SPEAKER
        // ---------------------------------------------------------------
        /// <summary>
        /// Updates an existing speaker by ID.
        /// </summary>
        /// <param name="speakerId">ID of the speaker.</param>
        /// <param name="request">Updated speaker details.</param>
        /// <returns>Status code and message.</returns>
        [HttpPut("update-speaker/{speakerId}")]
        public async Task<IActionResult> UpdateSpeaker(int speakerId, [FromForm] NewInitiativeSpeakerRequest request)
        {
            var (code, message) = await _speakerService.UpdateSpeakerAsync(speakerId, request);
            return StatusCode(code, new { Message = message });
        }

        // ---------------------------------------------------------------
        // 🔹 GET SPEAKER BY ID
        // ---------------------------------------------------------------
        /// <summary>
        /// Retrieves details of a specific speaker by ID.
        /// </summary>
        /// <param name="speakerId">Speaker ID.</param>
        /// <returns>Speaker details.</returns>
        [HttpGet("get-speaker/{speakerId}")]
        public async Task<IActionResult> GetSpeakerById(int speakerId)
        {
            var result = await _speakerService.GetSpeakerByIdAsync(speakerId);

            if (result == null)
                return NotFound(new { Message = "No speaker found." });

            return Ok(result);
        }

        // ---------------------------------------------------------------
        // 🔹 GET ALL SPEAKERS(LISTING + PAGINATION)
        // ---------------------------------------------------------------
       
        //[HttpGet("get-speakers")]
        //public async Task<IActionResult> GetSpeakers(
        //    [FromQuery] string? name,
        //    [FromQuery] string? designation,
        //    [FromQuery] bool? isActive,
        //    [FromQuery][Range(1, 9999)] int pageIndex = 1,
        //    [FromQuery][Range(1, 50)] int pageSize = 10)
        //{
        //    var result = await _speakerService.GetAllSpeakersAsync(
        //        name,
        //        designation,
        //        isActive,
        //        pageIndex,
        //        pageSize
        //    );

        //    if (result.Data.Count == 0)
        //        return Ok(new { Message = "No records found.", Data = new List<object>() });

        //    return Ok(result);
        //}
    }
}
