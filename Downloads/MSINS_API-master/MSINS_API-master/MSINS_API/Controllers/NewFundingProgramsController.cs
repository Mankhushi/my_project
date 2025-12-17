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
    public class NewFundingProgramsController : ControllerBase
    {
        private readonly INewFundingProgramsService _fundingService;

        public NewFundingProgramsController(INewFundingProgramsService fundingService)
        {
            _fundingService = fundingService;
        }

        /// <summary>
        /// Adds a new funding program.
        /// </summary>
        /// <param name="request">The funding program details including Logo, Agency Name, and URL.</param>
        /// <remarks>
        /// <para><b>File Upload:</b></para>
        /// <para>- Upload Logo as PNG/JPG.</para>
        /// 
        /// <para><b>Fields:</b></para>
        /// <para>- <c>FundingAgencyName</c>: Required</para>
        /// <para>- <c>WebsiteLink</c>: Optional link</para>
        /// <para>- <c>IsActive</c>: true or false</para>
        /// </remarks>
        /// <returns>Status code 200 on success, otherwise proper error code.</returns>
        [HttpPost("add-funding-program")]
        public async Task<IActionResult> AddFundingProgram([FromForm] NewFundingProgramsRequest request)
        {
            // Trigger validation
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

            var (statusCode, message) = await _fundingService.AddFundingProgramAsync(request);
            return StatusCode(statusCode, new { Message = message });
        }

        /// <summary>
        /// Updates an existing funding program.
        /// </summary>
        /// <param name="fundProgramId">The ID of the funding program to be updated.</param>
        /// <param name="request">The new details of the funding program.</param>
        /// <returns>Status code 200 on success, or relevant error code.</returns>
        [HttpPut("update-funding-program/{fundProgramId}")]
        public async Task<IActionResult> UpdateFundingProgram(int fundProgramId, [FromForm] NewFundingProgramsRequest request)
        {
            if (fundProgramId <= 0)
                return BadRequest(new { message = "Invalid FundProgramId." });

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

            var (statusCode, message) = await _fundingService.UpdateFundingProgramAsync(fundProgramId, request);
            return StatusCode(statusCode, new { Message = message });
        }

        /// <summary>
        /// Retrieves a paginated list of funding programs with optional filters.
        /// </summary>
        /// <param name="isActive">Filter by active/inactive status.</param>
        /// <param name="pageIndex">Page number (default: 1).</param>
        /// <param name="pageSize">Records per page (default: 10).</param>
        /// <returns>A paginated list of funding programs.</returns>
        [HttpGet("getall-funding-programs")]
        public async Task<IActionResult> GetFundingPrograms(
     [FromQuery] string? fundingAgencyName,
     [FromQuery] bool? isActive,
     [FromQuery] int pageIndex = 1,
     [FromQuery] int pageSize = 10)
        {
            var result = await _fundingService.GetFundingProgramsAsync(fundingAgencyName, isActive, pageIndex, pageSize);

            if (result == null || result.Data == null || result.Data.Count == 0)
            {
                return Ok(new
                {
                    message = "No records found.",
                    data = new List<object>(),
                    totalCount = 0,
                    pageIndex,
                    pageSize
                });
            }

            return Ok(new
            {
                message = "Funding programs fetched successfully.",
                data = result.Data,
                totalCount = result.TotalRecords,
                pageIndex,
                pageSize
            });
        }


        /// <summary>
        /// Retrieves funding program details by ID.
        /// </summary>
        /// <param name="fundProgramId">The ID of the funding program.</param>
        /// <returns>The funding program details.</returns>
        [HttpGet("get-funding-program/{fundProgramId}")]
        public async Task<IActionResult> GetFundingProgramById(int fundProgramId)
        {
            var result = await _fundingService.GetFundingProgramByIdAsync(fundProgramId);

            if (result == null)
                return NotFound(new { message = "Funding program not found." });

            return Ok(result);
        }
    }
}
