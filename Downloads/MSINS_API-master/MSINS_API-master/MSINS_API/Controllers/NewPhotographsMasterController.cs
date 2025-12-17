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
    public class NewPhotographsMasterController : ControllerBase
    {
        private readonly INewPhotographsMasterService _photoService;

        public NewPhotographsMasterController(INewPhotographsMasterService photoService)
        {
            _photoService = photoService;
        }

        // ============================================================
        //                        ADD PHOTOGRAPH
        // ============================================================
        [HttpPost("add-photograph")]
        public async Task<IActionResult> AddPhotograph(
            [FromForm] NewPhotographsMasterRequest request)
        {
            // Model validation
            var ctx = new ValidationContext(request);
            var results = new List<ValidationResult>();

            if (!Validator.TryValidateObject(request, ctx, results, true))
            {
                return BadRequest(new
                {
                    message = "Validation failed.",
                    errors = results.Select(x => x.ErrorMessage)
                });
            }

            var (status, message) = await _photoService.AddPhotographAsync(request);

            return StatusCode(status, new { Message = message });
        }

        // ============================================================
        //                        UPDATE PHOTOGRAPH
        // ============================================================
        [HttpPut("update-photograph/{photographId}")]
        public async Task<IActionResult> UpdatePhotograph(
            int photographId,
            [FromForm] NewPhotographsMasterRequest request)
        {
            if (photographId <= 0)
                return BadRequest(new { message = "Invalid PhotographId." });

            // Validate request model
            var ctx = new ValidationContext(request);
            var results = new List<ValidationResult>();

            if (!Validator.TryValidateObject(request, ctx, results, true))
            {
                return BadRequest(new
                {
                    message = "Validation failed.",
                    errors = results.Select(x => x.ErrorMessage)
                });
            }

            var (status, message) =
                await _photoService.UpdatePhotographAsync(photographId, request);

            return StatusCode(status, new { Message = message });
        }

        // ============================================================
        //                        GET ALL PHOTOGRAPHS
        // ============================================================
        [HttpGet("getall-photographs")]
        public async Task<IActionResult> GetPhotographs(
            [FromQuery][Required] int? initiativeId,
            [FromQuery] bool? isActive = true)
        {
            if (initiativeId == null || initiativeId <= 0)
                return BadRequest(new { message = "initiativeId must be greater than 0." });

            var result = await _photoService.GetPhotographsAsync(initiativeId.Value, isActive);

            return Ok(new
            {
                message = "Photographs fetched successfully.",
                data = result
            });
        }

        // ============================================================
        //                        GET BY ID
        // ============================================================
        [HttpGet("get-photograph/{photographId}")]
        public async Task<IActionResult> GetPhotographById(int photographId)
        {
            var result = await _photoService.GetPhotographByIdAsync(photographId);

            if (result == null)
                return NotFound(new { message = "Photograph not found." });

            return Ok(result);
        }
    }
}
