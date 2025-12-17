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
    public class NewFaqsController : ControllerBase
    {
        private readonly INewFaqsService _faqsService;

        public NewFaqsController(INewFaqsService faqsService)
        {
            _faqsService = faqsService;
        }

        // =====================================================================
        //                             ADD FAQ
        // =====================================================================
        [HttpPost("add-faq")]
        public async Task<IActionResult> AddFaq([FromForm] NewFaqsRequest request)
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

            var (statusCode, message) = await _faqsService.AddFaqAsync(request);
            return StatusCode(statusCode, new { Message = message });
        }

        // =====================================================================
        //                             UPDATE FAQ
        // =====================================================================
        [HttpPut("update-faq/{faqsId}")]
        public async Task<IActionResult> UpdateFaq(int faqsId, [FromForm] NewFaqsRequest request)
        {
            if (faqsId <= 0)
                return BadRequest(new { message = "Invalid FAQ ID." });

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

            var (statusCode, message) = await _faqsService.UpdateFaqAsync(faqsId, request);
            return StatusCode(statusCode, new { Message = message });
        }

        // =====================================================================
        //                             GET FAQS
        // =====================================================================
        [HttpGet("getall-faqs")]
        public async Task<IActionResult> GetFaqs(
    [FromQuery][Required] int? initiativeId,   // ⭐ Mandatory
    [FromQuery] bool? isActive = true          // ⭐ Default TRUE
)
        {
            // Validate initiativeId
            if (initiativeId == null || initiativeId <= 0)
            {
                return BadRequest(new
                {
                    message = "initiativeId is required and must be greater than 0."
                });
            }

            // Service call
            var result = await _faqsService.GetFaqsAsync(initiativeId.Value, isActive);

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
                message = "FAQs fetched successfully.",
                data = result
            });
        }


        // =====================================================================
        //                             GET FAQ BY ID
        // =====================================================================
        [HttpGet("get-faq/{faqsId}")]
        public async Task<IActionResult> GetFaqById(int faqsId)
        {
            var result = await _faqsService.GetFaqByIdAsync(faqsId);

            if (result == null)
                return NotFound(new { message = "FAQ not found." });

            return Ok(result);
        }
    }
}
