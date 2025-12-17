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
    public class NewInitiativeMasterController : ControllerBase
    {
        private readonly INewInitiativeMasterService _initiativeService;

        public NewInitiativeMasterController(INewInitiativeMasterService initiativeService)
        {
            _initiativeService = initiativeService;
        }

        // ------------------------------------------------------------
        // ⭐ ADD NEW INITIATIVE
        // ------------------------------------------------------------
        [HttpPost("add")]
        public async Task<IActionResult> AddNewInitiative([FromForm] NewInitiativeMasterRequest request)
        {
            var validation = ValidateRequest(request);
            if (validation != null)
                return validation;

            var (statusCode, message) = await _initiativeService.AddInitiativeMasterAsync(request);

            return StatusCode(statusCode, new
            {
                Success = statusCode == 200,
                Message = message
            });
        }

        // ------------------------------------------------------------
        // ⭐ UPDATE INITIATIVE
        // ------------------------------------------------------------
        [HttpPut("update/{id:int}")]
        public async Task<IActionResult> UpdateNewInitiative(int id, [FromForm] NewInitiativeMasterRequest request)
        {
            var validation = ValidateRequest(request);
            if (validation != null)
                return validation;

            var (statusCode, message) = await _initiativeService.UpdateInitiativeMasterAsync(id, request);

            return StatusCode(statusCode, new
            {
                Success = statusCode == 200,
                Message = message
            });
        }

        // ------------------------------------------------------------
        // ⭐ GET BY ID
        // ------------------------------------------------------------
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetInitiativeById(int id)
        {
            var result = await _initiativeService.GetInitiativeMasterByIdAsync(id);

            if (result == null)
                return NotFound(new { message = "Record not found." });

            return Ok(result);
        }

        // ------------------------------------------------------------
        // ⭐ GET ALL (SEARCH + FILTER + PAGINATION)
        // ------------------------------------------------------------
        [HttpGet("list")]
        public async Task<IActionResult> GetInitiatives(
      [FromQuery] string? title,
      [FromQuery] bool? isActive = true,
      [FromQuery] int pageIndex = 1,
      [FromQuery] int pageSize = 10)
        {
            var result = await _initiativeService.GetInitiativeMasterAsync(title, isActive, pageIndex, pageSize);

            return Ok(new
            {
                data = result.Data,
                totalRecords = result.TotalRecords
            });
        }


        // ------------------------------------------------------------
        // ⭐ Shared Validation Method
        // ------------------------------------------------------------
        private IActionResult? ValidateRequest(NewInitiativeMasterRequest request)
        {
            var context = new ValidationContext(request, null, null);
            var results = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(request, context, results, true);

            if (!isValid)
            {
                return BadRequest(new
                {
                    message = "Validation failed.",
                    errors = results.Select(e => e.ErrorMessage)
                });
            }

            return null;
        }
    }
}
