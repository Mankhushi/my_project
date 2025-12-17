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
    public class NewTestimonialsMasterController : ControllerBase
    {
        private readonly INewTestimonialsMasterService _service;

        public NewTestimonialsMasterController(INewTestimonialsMasterService service)
        {
            _service = service;
        }

        // =====================================================================
        //                       ADD TESTIMONIAL
        // =====================================================================
        [HttpPost("add-testimonial")]
        public async Task<IActionResult> AddTestimonial(
            [FromForm] NewTestimonialsMasterRequest request)
        {
            // Basic model validation
            var context = new ValidationContext(request);
            var results = new List<ValidationResult>();

            if (!Validator.TryValidateObject(request, context, results, validateAllProperties: true))
            {
                return BadRequest(new
                {
                    message = "Validation failed.",
                    errors = results.Select(x => x.ErrorMessage)
                });
            }

            // Call service (service handles file upload)
            var (status, message) = await _service.AddTestimonialAsync(request);

            return StatusCode(status, new { Message = message });
        }

        // =====================================================================
        //                       UPDATE TESTIMONIAL
        // =====================================================================
        [HttpPut("update-testimonial/{testimonialId}")]
        public async Task<IActionResult> UpdateTestimonial(
            int testimonialId,
            [FromForm] NewTestimonialsMasterRequest request)
        {
            if (testimonialId <= 0)
                return BadRequest(new { message = "Invalid TestimonialId." });

            var context = new ValidationContext(request);
            var results = new List<ValidationResult>();

            if (!Validator.TryValidateObject(request, context, results, validateAllProperties: true))
            {
                return BadRequest(new
                {
                    message = "Validation failed.",
                    errors = results.Select(x => x.ErrorMessage)
                });
            }

            var (status, message) = await _service.UpdateTestimonialAsync(testimonialId, request);

            return StatusCode(status, new { Message = message });
        }

        // =====================================================================
        //                       GET ALL TESTIMONIALS
        // =====================================================================
        [HttpGet("getall-testimonials")]
        public async Task<IActionResult> GetTestimonials(
            [FromQuery][Required] int? initiativeId,
            [FromQuery] bool? isActive = true)
        {
            if (initiativeId == null || initiativeId <= 0)
                return BadRequest(new { message = "initiativeId must be greater than 0." });

            var result = await _service.GetTestimonialsAsync(initiativeId.Value, isActive);

            if (result == null || result.Count == 0)
            {
                return Ok(new
                {
                    message = "No testimonials found.",
                    data = new List<object>()
                });
            }

            return Ok(new
            {
                message = "Testimonials fetched successfully.",
                data = result
            });
        }

        // =====================================================================
        //                       GET TESTIMONIAL BY ID
        // =====================================================================
        [HttpGet("get-testimonial/{testimonialId}")]
        public async Task<IActionResult> GetTestimonialById(int testimonialId)
        {
            if (testimonialId <= 0)
                return BadRequest(new { message = "Invalid TestimonialId." });

            var result = await _service.GetTestimonialByIdAsync(testimonialId);

            if (result == null)
                return NotFound(new { message = "Testimonial not found." });

            return Ok(result);
        }
    }
}
