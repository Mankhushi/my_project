using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MSINS_API.Models.Response;
using MSINS_API.Services.Interface;

namespace MSINS_API.Controllers
{
    [ApiVersion("1.0")]
    //[Route("msins/[controller]")]
    [Route("msins/v{version:apiVersion}/[controller]")] // Versioned route
    [ApiExplorerSettings(GroupName = "v1")]    
    [ApiController]
    [Authorize(Policy = "AdminPolicy")] // globally apply controller

    public class WebFormsController : ControllerBase
    {
        
        private readonly IPublicConsultationListService _Listservice;
        private readonly IFeedbackService _Feedbackservice;
        private readonly IGrievanceService _Grievanceservice;
        private readonly ISuggestionService _Suggestionservice;

        public WebFormsController(IPublicConsultationListService Listservice, 
            IFeedbackService Feedbackservice, IGrievanceService Grievanceservice, ISuggestionService SuggestionService)
        {
            _Listservice = Listservice;
            _Feedbackservice = Feedbackservice;
            _Grievanceservice = Grievanceservice;
            _Suggestionservice = SuggestionService;
        }

        /// <summary>
        /// Retrieves a paginated list of Public Consultation.
        /// </summary>
        /// <param name="pageIndex">The page number (default is 1, must be greater than 0).</param>
        /// <param name="pageSize">The number of records per page (default is 10, must be greater than 0).</param>
        /// <param name="searchTerm">Search term that filters results by name, email, or contact number.</param>
        /// <returns>A paginated list of records or a message if no records are found.</returns>
        [HttpGet("public-consultations")]
        public async Task<IActionResult> GetPublicConsultationList([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10, [FromQuery] string searchTerm = "", [FromQuery] bool isExport = false)
        {
            if (pageIndex < 1 || pageSize < 1)
                return BadRequest("PageIndex and PageSize must be greater than 0");

            var result = await _Listservice.GetPublicConsultationAll(pageIndex, pageSize, searchTerm, isExport);
            
            if (result.Data.Count.Equals(0))
                return Ok(new { message = "No records found.", data = new List<object>() });

            return Ok(result);
        }

        /// <summary>
        /// Retrieves a paginated list of Feedback.
        /// </summary>
        /// <param name="pageIndex">The page number (default is 1, must be greater than 0).</param>
        /// <param name="pageSize">The number of records per page (default is 10, must be greater than 0).</param>
        /// <param name="searchTerm">Search term that filters results by name, email, or contact number.</param>
        /// <returns>A paginated list of records or a message if no records are found.</returns>
        [HttpGet("feedbacks")]
        public async Task<IActionResult> GetAllFeedback([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10, [FromQuery] string searchTerm = "", [FromQuery] bool isExport = false)
        {
            if (pageIndex < 1 || pageSize < 1)
                return BadRequest("PageIndex and PageSize must be greater than 0");

            var result = await _Feedbackservice.GetAllFeedback(pageIndex, pageSize, searchTerm, isExport);

            if (result.Data.Count.Equals(0))
                return Ok(new { message = "No records found.", data = new List<object>() });

            return Ok(result);
        }

        /// <summary>
        /// Retrieves a paginated list of Grievance.
        /// </summary>
        /// <param name="pageIndex">The page number (default is 1, must be greater than 0).</param>
        /// <param name="pageSize">The number of records per page (default is 10, must be greater than 0).</param>
        /// <param name="searchTerm">Search term that filters results by name, email, or contact number.</param>
        /// <returns>A paginated list of records or a message if no records are found.</returns>
        [HttpGet("grievances")]
        public async Task<IActionResult> GetAllGrievance([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10, [FromQuery] string searchTerm = "", [FromQuery] bool isExport = false)
        {
            if (pageIndex < 1 || pageSize < 1)
                return BadRequest("PageIndex and PageSize must be greater than 0");

            var result = await _Grievanceservice.GetAllGrievance(pageIndex, pageSize, searchTerm, isExport);

            if (result.Data.Count.Equals(0))
                return Ok(new { message = "No records found.", data = new List<object>() });

            return Ok(result);
        }

        /// <summary>
        /// Retrieves a paginated list of Suggestion.
        /// </summary>
        /// <param name="pageIndex">The page number (default is 1, must be greater than 0).</param>
        /// <param name="pageSize">The number of records per page (default is 10, must be greater than 0).</param>
        /// <param name="searchTerm">Search term that filters results by name, email, or contact number.</param>
        /// <returns>A paginated list of records or a message if no records are found.</returns>
        [HttpGet("suggestions")]
        public async Task<IActionResult> GetAllSuggestion([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10, [FromQuery] string searchTerm = "", [FromQuery] bool isExport = false)
        {
            if (pageIndex < 1 || pageSize < 1)
                return BadRequest("PageIndex and PageSize must be greater than 0");

            var result = await _Suggestionservice.GetAllSuggestion(pageIndex, pageSize, searchTerm, isExport);

            if (result.Data.Count.Equals(0))
                return Ok(new { message = "No records found.", data = new List<object>() });

            return Ok(result);
        }
    }
}
