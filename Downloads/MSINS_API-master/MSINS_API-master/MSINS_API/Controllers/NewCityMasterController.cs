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
    public class NewCityMasterController : ControllerBase
    {
        private readonly INewCityMasterService _cityService;

        public NewCityMasterController(INewCityMasterService cityService)
        {
            _cityService = cityService;
        }

        //---------------- ADD CITY-------------------------------


        // ---------------UPDATE CITY-----------------------------
        /// <summary>
        /// Updates an existing city.
        /// </summary>
        /// <param name="cityId">City ID to update.</param>
        /// <param name="dto">Updated city details.</param>
        /// <remarks>
        /// <para><b>Validation:</b></para>
        /// <para>- City must exist.</para>
        /// <para>- City name cannot duplicate another city.</para>
        /// </remarks>
        /// <returns>Response code and status message.</returns>
        /// <response code="200">City updated successfully.</response>
        /// <response code="400">Invalid input or duplicate name.</response>
        /// <response code="404">City not found.</response>
        /// <response code="500">Unexpected server error.</response>
        [HttpPut("update-city/{cityId}")]
        public async Task<IActionResult> UpdateCity(
            [Required] int cityId,
            [FromForm] NewCityMasterRequest dto)
        {
            var (status, message) = await _cityService.UpdateCityAsync(cityId, dto);

            return StatusCode(status, new { Message = message });
        }

        // --------------------GET CITY BY ID----------------------------------------------
        /// <summary>
        /// Returns city details by ID.
        /// </summary>
        /// <param name="cityId">City ID.</param>
        /// <returns>City details.</returns>
        [HttpGet("get-city/{cityId}")]
        public async Task<IActionResult> GetCityById([Required] int cityId)
        {
            var result = await _cityService.GetCityByIdAsync(cityId);

            if (result == null)
                return NotFound(new { Message = "City not found." });

            return Ok(new { StatusCode = 200, Data = result });
        }
    }
}
