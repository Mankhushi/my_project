using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MSINS_API.Models.Request;
using MSINS_API.Services.Interface;

namespace MSINS_API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    //[Route("msins/public-consultation")] // Default route without version
    [Route("msins/v{version:apiVersion}/public-consultation")] // Versioned route
    [ApiExplorerSettings(GroupName = "v1")]
    public class PublicConsultationController : ControllerBase
    {
        private readonly IPublicConsultationService _service;
        private readonly IPublicConsultationListService _Listservice;

        public PublicConsultationController(IPublicConsultationService service, IPublicConsultationListService Listservice)
        {
            _service = service;
            _Listservice = Listservice;
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
