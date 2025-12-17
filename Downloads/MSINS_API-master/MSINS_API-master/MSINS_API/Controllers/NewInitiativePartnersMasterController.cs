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
    public class NewInitiativePartnersMasterController : ControllerBase
    {
        private readonly INewInitiativePartnersMasterService _partnersService;

        public NewInitiativePartnersMasterController(INewInitiativePartnersMasterService partnersService)
        {
            _partnersService = partnersService;
        }

        // ADD PARTNER
        [HttpPost("add-partner")]
        public async Task<IActionResult> AddPartner([FromForm] NewInitiativePartnersMasterRequest request)
        {
            var (code, message) = await _partnersService.AddPartnerAsync(request, null);

            return StatusCode(code, new { Message = message });
        }

        // UPDATE PARTNER
        [HttpPut("update-partner/{partnerId}")]
        public async Task<IActionResult> UpdatePartner(int partnerId, [FromForm] NewInitiativePartnersMasterRequest request)
        {
            var (code, message) =
                await _partnersService.UpdatePartnerAsync(partnerId, request, null);

            return StatusCode(code, new { Message = message });
        }

        // GET ALL
        [HttpGet("getall-partners")]
        public async Task<IActionResult> GetPartners(bool? isActive = true, int? initiativeId = null)
        {
            var partners = await _partnersService.GetPartnersAsync(initiativeId, isActive);

            return Ok(new
            {
                message = "Partners fetched successfully.",
                data = partners
            });
        }

        // GET BY ID
        [HttpGet("get-partner/{partnerId}")]
        public async Task<IActionResult> GetPartnerById(int partnerId)
        {
            var partner = await _partnersService.GetPartnerByIdAsync(partnerId);

            if (partner == null)
                return NotFound(new { message = "Partner not found" });

            return Ok(partner);
        }
    }
}
