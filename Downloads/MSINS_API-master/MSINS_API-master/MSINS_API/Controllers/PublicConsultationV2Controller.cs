using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MSINS_API.Models.Request;
using MSINS_API.Services.Interface;

namespace MSINS_API.Controllers
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("msins/v{version:apiVersion}/public-consultation")]
    [ApiExplorerSettings(GroupName = "v2")]
    public class PublicConsultationV2Controller : ControllerBase
    {
        private readonly IPublicConsultationService _service;

        public PublicConsultationV2Controller(IPublicConsultationService service)
        {
            _service = service;
        }

        /// <summary>
        /// Save the data of Public Consultation 
        /// </summary>
        [HttpPost("send-consultation")]
        [Authorize(Policy = "AllPolicy")]
        public async Task<IActionResult> SubmitPublicConsultation([FromForm] PublicationConsultationRequest model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid input data.");
            }

            string result = await _service.SubmitPublicConsultationAsync(model);

            if (result == "Submission successful.")
            {
                return Ok(result);
            }

            return StatusCode(500, result);
        }
    }
}
