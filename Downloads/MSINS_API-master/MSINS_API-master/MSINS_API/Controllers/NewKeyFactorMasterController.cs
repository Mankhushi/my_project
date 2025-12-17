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
    public class NewKeyFactorMasterController : ControllerBase
    {
        private readonly INewKeyFactorMasterService _keyFactorService;

        public NewKeyFactorMasterController(INewKeyFactorMasterService keyFactorService)
        {
            _keyFactorService = keyFactorService;
        }

        // ============================================================
        //                          ADD KEY FACTOR
        // ============================================================
        [HttpPost("add-keyfactor")]
        public async Task<IActionResult> AddKeyFactor([FromForm] NewKeyFactorMasterRequest request)
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

            var (statusCode, message) = await _keyFactorService.AddKeyFactorAsync(request);
            return StatusCode(statusCode, new { Message = message });
        }

        // ============================================================
        //                         UPDATE KEY FACTOR
        // ============================================================
        [HttpPut("update-keyfactor/{keyFactorId}")]
        public async Task<IActionResult> UpdateKeyFactor(int keyFactorId, [FromForm] NewKeyFactorMasterRequest request)
        {
            if (keyFactorId <= 0)
                return BadRequest(new { message = "Invalid KeyFactorId." });

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

            var (statusCode, message) = await _keyFactorService.UpdateKeyFactorAsync(keyFactorId, request);
            return StatusCode(statusCode, new { Message = message });
        }

        // ============================================================
        //                          GET ALL KEY FACTORS
        // ============================================================
        [HttpGet("getall-keyfactors")]
        public async Task<IActionResult> GetKeyFactors(
    [FromQuery] bool? isActive = true,
    [FromQuery] int? initiativeId = null)
        {
            var result = await _keyFactorService.GetKeyFactorsAsync(isActive, initiativeId);

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
                message = "Key factors fetched successfully.",
                data = result
            });
        }


        // ============================================================
        //                         GET KEY FACTOR BY ID
        // ============================================================
        [HttpGet("get-keyfactor/{keyFactorId}")]
        public async Task<IActionResult> GetKeyFactorById(int keyFactorId)
        {
            var result = await _keyFactorService.GetKeyFactorByIdAsync(keyFactorId);

            if (result == null)
                return NotFound(new { message = "Key factor not found." });

            return Ok(result);
        }
    }
}
