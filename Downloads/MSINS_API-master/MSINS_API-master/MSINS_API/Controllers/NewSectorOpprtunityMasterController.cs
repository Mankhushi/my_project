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
    public class NewSectorOpprtunityMasterController : ControllerBase
    {
        private readonly INewSectorOpprtunityMasterService _sectorOpprtunityService;

        public NewSectorOpprtunityMasterController(INewSectorOpprtunityMasterService sectorOpprtunityService)
        {
            _sectorOpprtunityService = sectorOpprtunityService;
        }

        // ============================================================
        //                     ADD SECTOR OPPRTUNITY
        // ============================================================
        [HttpPost("add-sector-opprtunity")]
        public async Task<IActionResult> AddSectorOpprtunity(
            [FromForm] NewSectorOpprtunityMasterRequest request)
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

            var (statusCode, message) =
                await _sectorOpprtunityService.AddSectorOpprtunityAsync(request);

            return StatusCode(statusCode, new { Message = message });
        }

        // ============================================================
        //                    UPDATE SECTOR OPPRTUNITY
        // ============================================================
        [HttpPut("update-sector-opprtunity/{sectorOpprtunityId}")]
        public async Task<IActionResult> UpdateSectorOpprtunity(
            int sectorOpprtunityId,
            [FromForm] NewSectorOpprtunityMasterRequest request)
        {
            if (sectorOpprtunityId <= 0)
                return BadRequest(new { message = "Invalid SectorOpprtunityId." });

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

            var (statusCode, message) =
                await _sectorOpprtunityService.UpdateSectorOpprtunityAsync(sectorOpprtunityId, request);

            return StatusCode(statusCode, new { Message = message });
        }

        // ============================================================
        //                 GET ALL SECTOR OPPRTUNITY
        // ============================================================
        [HttpGet("getall-sector-opprtunity")]
        public async Task<IActionResult> GetSectorOpprtunity(
            [FromQuery] bool? isActive = true,
            [FromQuery] int? initiativeId = null)
        {
            var result = await _sectorOpprtunityService
                .GetSectorOpprtunityAsync(isActive, initiativeId);

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
                message = "Sector opportunities fetched successfully.",
                data = result
            });
        }

        // ============================================================
        //             GET SECTOR OPPRTUNITY BY ID
        // ============================================================
        [HttpGet("get-sector-opprtunity/{sectorOpprtunityId}")]
        public async Task<IActionResult> GetSectorOpprtunityById(int sectorOpprtunityId)
        {
            var result = await _sectorOpprtunityService
                .GetSectorOpprtunityByIdAsync(sectorOpprtunityId);

            if (result == null)
                return NotFound(new { message = "Sector opportunity not found." });

            return Ok(result);
        }
    }
}
