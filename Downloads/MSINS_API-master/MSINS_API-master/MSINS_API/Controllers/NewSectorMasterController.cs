using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MSINS_API.Models.Request;
using MSINS_API.Models.Response;
using MSINS_API.Services.Interface;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace MSINS_API.Controllers
{
    [ApiVersion("1.0")]
    [Route("msins/v{version:apiVersion}/[controller]")]
    [ApiExplorerSettings(GroupName = "v1")]
    [ApiController]
    [Authorize(Policy = "AdminPolicy")]
    public class NewSectorMasterController : ControllerBase
    {
        private readonly INewSectorMasterService _sectorService;

        public NewSectorMasterController(INewSectorMasterService sectorService)
        {
            _sectorService = sectorService;
        }

        /// <summary>
        /// Adds a new sector entry.
        /// </summary>
        /// <param name="sectorRequest">The sector details to be added.</param>
        /// <response code="200">Sector added successfully.</response>
        /// <response code="400">Validation failed.</response>
        /// <response code="500">Unexpected server error.</response>
        [HttpPost("save-sector")]
        public async Task<IActionResult> AddSector([FromBody] NewSectorMasterRequest sectorRequest)
        {
            // Validate request
            var context = new ValidationContext(sectorRequest, null, null);
            var results = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(sectorRequest, context, results, true);

            if (!isValid)
            {
                return BadRequest(new
                {
                    message = "Validation failed.",
                    errors = results.Select(e => e.ErrorMessage)
                });
            }

            var (statusCode, message) = await _sectorService.AddSectorAsync(sectorRequest);
            return StatusCode(statusCode, new { Message = message });
        }

        /// <summary>
        /// Updates an existing sector entry by ID.
        /// </summary>
        /// <param name="id">The ID of the sector to update.</param>
        /// <param name="sectorRequest">Updated sector details.</param>
        /// <response code="200">Sector updated successfully.</response>
        /// <response code="400">Validation failed or invalid ID.</response>
        /// <response code="404">Sector not found.</response>
        /// <response code="500">Unexpected server error.</response>
        [HttpPut("update-sector/{id}")]
        public async Task<IActionResult> UpdateSector(int id, [FromBody] NewSectorMasterRequest sectorRequest)
        {
            if (id <= 0)
                return BadRequest(new { message = "Invalid SectorId." });

            sectorRequest.SectorId = id;

            var (code, message) = await _sectorService.UpdateSectorAsync(sectorRequest);

            if (code == 400)
                return BadRequest(new { Message = message });
            if (code == 0)
                return NotFound(new { Message = message });

            return Ok(new { Code = code, Message = message });
        }



        // Get By ID ----------------------------------------->>>>>>>>

        /// <summary>
        /// Retrieves a sector by its ID.
        /// </summary>
        /// <param name="id">The ID of the sector to retrieve.</param>
        /// <returns>Returns a sector record if found, or 404 if not found.</returns>
        [HttpGet("get-sector/{id}")]
        public async Task<IActionResult> GetSectorById(int id)
        {
            if (id <= 0)
                return BadRequest(new { message = "Invalid SectorId." });

            var sector = await _sectorService.GetSectorByIdAsync(id);

            if (sector == null)
                return NotFound(new { message = "Sector not found." });

            return Ok(sector);
        }

        ///// <summary>
        ///// ✅ GET ALL(Listing + Searching)
        ///// </summary>
        ///// <param name="pageNumber"></param>
        ///// <param name="pageSize"></param>
        ///// <param name="searchText"></param>
        ///// <param name="isActive"></param>
        ///// <returns></returns>

        //[HttpGet("get-all")]
        //public async Task<IActionResult> GetAllSectors(
        ////[FromQuery] int pageNumber = 1,
        ////[FromQuery] int pageSize = 10,
        ////[FromQuery] string? searchText = null,
        //[FromQuery] bool? isActive = null)
        //{
        //    var result = await _sectorService.GetPaginatedSectorsAsync(isActive);

        //    return Ok(new
        //    {
        //        Status = "Success",
        //        Data = result.Data,
        //        //TotalRecords = result.TotalRecords,
        //        //PageNumber = pageNumber,
        //        //PageSize = pageSize
        //    });
        //}



    }
}
