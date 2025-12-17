using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MSINS_API.Models.Request;
using MSINS_API.Services.Implementation;
using MSINS_API.Services.Interface;
using System.ComponentModel.DataAnnotations;

namespace MSINS_API.Controllers
{
    [ApiVersion("1.0")]
    [Route("msins/v{version:apiVersion}/[controller]")]
    [ApiExplorerSettings(GroupName = "v1")]
    [ApiController]
    [Authorize(Policy = "AdminPolicy")]

    public class NewIncubatorsMasterController : ControllerBase
    {
        private readonly INewIncubatorsMasterService _incubatorService;

        public NewIncubatorsMasterController(INewIncubatorsMasterService incubatorService)
        {
            _incubatorService = incubatorService;
        }


        //----------------Add-------------------------------------------
        /// <summary>
        /// Adds a new incubator entry.
        /// </summary>
        /// <param name="request">Incubator details including name, description, and logo file.</param>
        /// <remarks>
        /// <para><b>Image Requirements:</b></para>
        /// <para>- <c>Logo</c> must be JPG or PNG.</para>
        /// <para>- Max size allowed depends on your file upload logic.</para>
        ///
        /// <para><b>Fields:</b></para>
        /// <para>- <c>IncubatorName</c>: Name of the incubator.</para>
        /// <para>- <c>Description</c>: Description/details.</para>
        /// <para>- <c>Logo</c>: Image file.</para>
        /// </remarks>
        /// <returns>Returns success or failure message.</returns>
        /// <response code="200">Incubator added successfully.</response>
        /// <response code="400">Validation failed.</response>
        /// <response code="500">Unexpected server error.</response>

        [HttpPost("add")]
        public async Task<IActionResult> AddAsync([FromForm] NewIncubatorsMasterRequest request)
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

            var result = await _incubatorService.AddAsync(request);

            return Ok(result);
        }


        //----------------Update-------------------------------------------
        /// <summary>
        /// Updates an existing incubator entry.
        /// </summary>
        /// <param name="id">IncubatorId to update.</param>
        /// <param name="request">Updated incubator details.</param>
        /// <returns>Returns update result.</returns>
        /// <response code="200">Updated successfully.</response>
        /// <response code="400">Validation failed.</response>
        /// <response code="404">Record not found.</response>
        /// <response code="500">Unexpected server error.</response>

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromForm] NewIncubatorsMasterRequest request)
        {
            request.IncubatorId = id;

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

            var result = await _incubatorService.UpdateAsync(request);

            return Ok(result);
        }


        //----------------Get By Id-------------------------------------------
        /// <summary>
        /// Retrieves incubator details using IncubatorId.
        /// </summary>
        /// <param name="id">IncubatorId</param>
        /// <returns>Incubator details.</returns>
        /// <response code="200">Data retrieved successfully.</response>
        /// <response code="404">No record found.</response>

        [HttpGet("incubator/{id}")]
        public async Task<IActionResult> GetIncubatorById(int id)
        {
            var result = await _incubatorService.GetByIdAsync(id);

            if (result == null)
                return NotFound(new { Message = "No record found.", Data = (object?)null });

            return Ok(new
            {
                Message = "Record found",
                Data = result
            });
        }
    }
}
