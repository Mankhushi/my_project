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
    public class NewWinnerMasterController : ControllerBase
    {
        private readonly INewWinnerMasterService _winnerService;

        public NewWinnerMasterController(INewWinnerMasterService winnerService)
        {
            _winnerService = winnerService;
        }

        // ============================================================
        //                          ADD WINNER
        // ============================================================
        [HttpPost("add-winner")]
        public async Task<IActionResult> AddWinner([FromForm] NewWinnerMasterRequest request)
        {
            var context = new ValidationContext(request, null, null);
            var results = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(request, context, results, true);

            if (!isValid)
            {
                return BadRequest(new
                {
                    message = "Validation failed.",
                    errors = results.Select(r => r.ErrorMessage)
                });
            }

            var (statusCode, message) = await _winnerService.AddWinnerAsync(request);
            return StatusCode(statusCode, new { Message = message });
        }

        // ============================================================
        //                         UPDATE WINNER
        // ============================================================
        [HttpPut("update-winner/{winnerId}")]
        public async Task<IActionResult> UpdateWinner(int winnerId, [FromForm] NewWinnerMasterRequest request)
        {
            if (winnerId <= 0)
                return BadRequest(new { message = "Invalid WinnerId." });

            var context = new ValidationContext(request, null, null);
            var results = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(request, context, results, true);

            if (!isValid)
            {
                return BadRequest(new
                {
                    message = "Validation failed.",
                    errors = results.Select(r => r.ErrorMessage)
                });
            }

            var (statusCode, message) = await _winnerService.UpdateWinnerAsync(winnerId, request);
            return StatusCode(statusCode, new { Message = message });
        }

        // ============================================================
        //                        GET ALL WINNERS
        // ============================================================
        [HttpGet("getall-winners")]
        public async Task<IActionResult> GetWinners(
      [FromQuery] int? sectorId,
      [FromQuery] int? initiativeId,
      [FromQuery] bool? isActive = true)   // OPTIONAL PARAM LAST ✔
        {
            var result = await _winnerService.GetWinnersAsync(sectorId, isActive, initiativeId);

            if (result == null || result.Count == 0)
            {
                return Ok(new
                {
                    message = "No records found.",
                    data = new List<object>()
                });
            }

            return Ok(new
            {
                message = "Winners fetched successfully.",
                data = result
            });
        }

        // ============================================================
        //                        GET WINNER BY ID
        // ============================================================
        [HttpGet("get-winner/{winnerId}")]
        public async Task<IActionResult> GetWinnerById(int winnerId)
        {
            var result = await _winnerService.GetWinnerByIdAsync(winnerId);

            if (result == null)
                return NotFound(new { message = "Winner not found." });

            return Ok(result);
        }
    }
}
