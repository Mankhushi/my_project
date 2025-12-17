using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MSINS_API.Models.Request;
using MSINS_API.Services.Implementation;
using MSINS_API.Services.Interface;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;



namespace MSINS_API.Controllers
{
    [ApiVersion("1.0")]
    [Route("msins/v{version:apiVersion}/[controller]")] // Versioned route
    [ApiExplorerSettings(GroupName = "v1")]
    [ApiController]
    [Authorize(Policy = "AllPolicy")] // globally apply controller

    public class WebsiteController : ControllerBase
    {
        private readonly IWebsiteService _websiteService;
        private readonly INewEmailMasterService _emailService;
        private readonly INewIncubatorsMasterService _incubatorService;
        private readonly INewSectorMasterService _sectorService;
        private readonly INewCityMasterService _cityService;
        private readonly INewTypeMasterService _newTypeMasterService;
        private readonly INewContactMasterService _contactService;
        private readonly INewInitiativeSpeakerService _speakerService;
        private readonly INewFaqsService _faqsService;
        private readonly INewFundingProgramsService _fundingProgramsService;
        private readonly INewPhotographsMasterService _photoService;
        private readonly INewTestimonialsMasterService _service;
        private readonly INewWinnerMasterService _winnerService;
        private readonly INewKeyFactorMasterService _keyFactorService;
        private readonly INewSectorOpprtunityMasterService _sectorOpprtunityService;
        private readonly INewInitiativePartnersMasterService _partnersService;

        public WebsiteController(IWebsiteService WebsiteService, INewEmailMasterService emailService, INewIncubatorsMasterService incubatorService,
            INewSectorMasterService sectorService, INewCityMasterService cityService, INewTypeMasterService newTypeMasterService,
            INewContactMasterService contactService, INewInitiativeSpeakerService speakerService, INewFaqsService faqsService,
            INewFundingProgramsService fundingProgramsService, INewPhotographsMasterService photoService, INewTestimonialsMasterService service,
            INewWinnerMasterService winnerService, INewKeyFactorMasterService keyFactorService, INewSectorOpprtunityMasterService sectorOpprtunityService,
            INewInitiativePartnersMasterService partnersService)
        {
            _websiteService = WebsiteService;
            _emailService = emailService;
            _incubatorService = incubatorService;
            _sectorService = sectorService;
            _cityService = cityService;
            _newTypeMasterService = newTypeMasterService;
            _contactService = contactService;
            _speakerService = speakerService;
            _faqsService = faqsService;
            _fundingProgramsService = fundingProgramsService;
            _photoService = photoService;
            _service = service;
            _winnerService = winnerService;
            _keyFactorService = keyFactorService;
            _sectorOpprtunityService = sectorOpprtunityService;
            _partnersService = partnersService;
        }

        /// <summary>
        /// Retrieves a list of Active banners.
        /// </summary>
        /// <remarks>
        /// This endpoint fetches all banners from the database and includes a fully qualified image URL.
        /// </remarks>
        /// <response code="200">Successfully retrieved active banners. If no records found, returns an empty list with message: <c>"No active banners found."</c></response>
        /// <response code="500">Internal server error.</response>
        [HttpGet("get-active-banners")]
        public async Task<IActionResult> GetActiveBanners()
        {
            var banners = await _websiteService.GetActiveBanners();
            if (banners == null)
                return Ok(new { message = "No active banners found.", data = new List<object>() });

            return Ok(banners);
        }

        /// <summary>
        /// Retrieves a list of About Us details.
        /// </summary>
        /// <remarks>
        /// This endpoint fetches all About Us details from the database and includes fully qualified URLs for images and PDF files.
        /// </remarks>
        /// <response code="200">Successfully retrieved About Us details. If no records are found, returns an empty list with message: <c>"No About Us details found."</c></response>
        /// <response code="500">Internal server error.</response>
        [HttpGet("get-aboutus")]
        public async Task<IActionResult> GetAboutUsDetails()
        {
            var aboutUsDetails = await _websiteService.GetAboutUsDetailsAsync();
            if (aboutUsDetails == null || !aboutUsDetails.Any())
                return Ok(new { message = "No About Us details found.", data = new List<object>() });

            return Ok(aboutUsDetails);
        }

        /// <summary>
        /// Retrieves a list of active EcoSystem details.
        /// </summary>
        /// <remarks>
        /// This endpoint fetches all EcoSystem details from the database and includes fully qualified URLs for images.
        /// </remarks>
        /// <response code="200">Successfully retrieved EcoSystem details. If no records are found, returns an empty list with message: <c>"No EcoSystem details found."</c></response>
        /// <response code="500">Internal server error.</response>
        [HttpGet("get-ecosystem")]
        public async Task<IActionResult> GetEcoSystemDetails()
        {
            var ecoSystemDetails = await _websiteService.GetEcoSystemDetailsAsync();
            if (ecoSystemDetails == null || !ecoSystemDetails.Any())
                return Ok(new { message = "No EcoSystem details found.", data = new List<object>() });

            return Ok(ecoSystemDetails);
        }

        /// <summary>
        /// Retrieves a list of active Partners.
        /// </summary>
        /// <remarks>
        /// This endpoint fetches all partners from the database and includes fully qualified URLs for images.
        /// </remarks>
        /// <response code="200">Successfully retrieved partners. If no records are found, returns an empty list with message: <c>"No partners found."</c></response>
        /// <response code="500">Internal server error.</response>
        [HttpGet("get-partners")]
        public async Task<IActionResult> GetPartners()
        {
            var partners = await _websiteService.GetPartnersAsync();
            if (partners == null || !partners.Any())
                return Ok(new { message = "No partners found.", data = new List<object>() });

            return Ok(partners);
        }

        /// <summary>
        /// Retrieves a list of active leaders.
        /// </summary>
        /// <remarks>
        /// This endpoint fetches all leadership members from the database and includes fully qualified URLs for images.
        /// </remarks>
        /// <response code="200">Successfully retrieved leaders. If no records are found, returns an empty list with message: <c>"No leaders found."</c></response>
        /// <response code="500">Internal server error.</response>
        [HttpGet("get-leaders")]
        public async Task<IActionResult> GetLeaders()
        {
            var leaders = await _websiteService.GetLeadersAsync();
            if (leaders == null || !leaders.Any())
                return Ok(new { message = "No leaders found.", data = new List<object>() });

            return Ok(leaders);
        }

        /// <summary>
        /// Retrieves a list of active initiatives.
        /// </summary>
        /// <remarks>
        /// This endpoint fetches all active initiatives from the database.
        /// </remarks>
        /// <response code="200">Successfully retrieved initiatives. If no records are found, returns an empty list with message: <c>"No initiatives found."</c></response>
        /// <response code="500">Internal server error.</response>
        [HttpGet("get-initiatives")]
        public async Task<IActionResult> GetInitiatives()
        {
            var initiatives = await _websiteService.GetInitiativeAsync();

            if (initiatives == null || !initiatives.Any())
                return Ok(new { message = "No initiatives found.", data = new List<object>() });

            return Ok(initiatives);
        }

        /// <summary>
        /// Retrieves a list of active executives.
        /// </summary>
        /// <remarks>
        /// This endpoint fetches all active executives from the database.
        /// </remarks>
        /// <response code="200">Successfully retrieved executives. If no records are found, returns an empty list with message: <c>"No executives found."</c></response>
        /// <response code="500">Internal server error.</response>
        [HttpGet("get-executives")]
        public async Task<IActionResult> GetExecutives()
        {
            var executives = await _websiteService.GetExecutiveAsync();

            if (executives == null || !executives.Any())
                return Ok(new { message = "No executives found.", data = new List<object>() });

            return Ok(executives);
        }

        /// <summary>
        /// Retrieves a list of active Governing Council.
        /// </summary>
        /// <remarks>
        /// This endpoint fetches all active Governing Council from the database.
        /// </remarks>
        /// <response code="200">Successfully retrieved Governing Council. If no records are found, returns an empty list with message: <c>"No governing councils found."</c></response>
        /// <response code="500">Internal server error.</response>
        [HttpGet("get-governing-council")]
        public async Task<IActionResult> GetGoverningCouncil()
        {
            var executives = await _websiteService.GetCouncilAsync();

            if (executives == null || !executives.Any())
                return Ok(new { message = "No governing councils found.", data = new List<object>() });

            return Ok(executives);
        }

        /// <summary>
        /// Retrieves a list of active General Body.
        /// </summary>
        /// <remarks>
        /// This endpoint fetches all active General Body from the database.
        /// </remarks>
        /// <response code="200">Successfully retrieved General Body. If no records are found, returns an empty list with message: <c>"No General Bodys found."</c></response>
        /// <response code="500">Internal server error.</response>
        [HttpGet("get-general-body")]
        public async Task<IActionResult> GetGeneralBody()
        {
            var executives = await _websiteService.GetgeneralBodyAsync();

            if (executives == null || !executives.Any())
                return Ok(new { message = "No General Bodys found.", data = new List<object>() });

            return Ok(executives);
        }

        /// <summary>
        /// Retrieves a list of active featured videos.
        /// </summary>
        /// <remarks>
        /// This endpoint fetches all active videos that are marked as "Featured" from the database.
        /// </remarks>
        /// <response code="200">Successfully retrieved active featured videos. If no records are found, returns an empty list with message: <c>"No featured videos found."</c></response>
        /// <response code="500">Internal server error.</response>
        [HttpGet("get-featured-videos")]
        public async Task<IActionResult> GetFeaturedVideos()
        {
            var videos = await _websiteService.GetFeaturedVideoAsync();

            if (videos == null || !videos.Any())
                return Ok(new { message = "No featured videos found.", data = new List<object>() });

            return Ok(videos);
        }

        /// <summary>
        /// Retrieves a list of active latest videos.
        /// </summary>
        /// <remarks>
        /// This endpoint fetches all active videos that are marked as "latest" from the database.
        /// </remarks>
        /// <response code="200">Successfully retrieved active latest videos. If no records are found, returns an empty list with message: <c>"No latest videos found."</c></response>
        /// <response code="500">Internal server error.</response>
        [HttpGet("get-latest-videos")]
        public async Task<IActionResult> GetlatestVideos()
        {
            var videos = await _websiteService.GetLatestVideoAsync();

            if (videos == null || !videos.Any())
                return Ok(new { message = "No latest videos found.", data = new List<object>() });

            return Ok(videos);
        }

        /// <summary>
        /// Retrieves a list of active featured resource videos.
        /// </summary>
        /// <remarks>
        /// This endpoint fetches all active featured resource that are marked as "featured" from the database.
        /// </remarks>
        /// <response code="200">Successfully retrieved active featured resource. If no records are found, returns an empty list with message: <c>"No featured resource found."</c></response>
        /// <response code="500">Internal server error.</response>
        [HttpGet("get-featured-resource")]
        public async Task<IActionResult> GetFeaturedResource()
        {
            var videos = await _websiteService.GetFeaturedResourceAsync();

            if (videos == null || !videos.Any())
                return Ok(new { message = "No featured videos found.", data = new List<object>() });

            return Ok(videos);
        }

        /// <summary>
        /// Retrieves a list of active government resolution videos.
        /// </summary>
        /// <remarks>
        /// This endpoint fetches all active government resolution that are marked as "latest" from the database.
        /// </remarks>
        /// <response code="200">Successfully retrieved active government resolution. If no records are found, returns an empty list with message: <c>"No government resolution found."</c></response>
        /// <response code="500">Internal server error.</response>
        [HttpGet("get-government-resolution")]
        public async Task<IActionResult> GetGovernmentResolution()
        {
            var videos = await _websiteService.GetGovernmentResolutionAsync();

            if (videos == null || !videos.Any())
                return Ok(new { message = "No Government Resolution found.", data = new List<object>() });

            return Ok(videos);
        }

        /// <summary>
        /// Retrieves a list of active Tender Notification.
        /// </summary>
        /// <remarks>
        /// This endpoint fetches all active Tender Notification from the database.
        /// </remarks>
        /// <response code="200">Successfully retrieved Tender Notification. If no records are found, returns an empty list with message: <c>"No Tender Notifications found."</c></response>
        /// <response code="500">Internal server error.</response>
        [HttpGet("get-tender-notification")]
        public async Task<IActionResult> GetTenderNotification()
        {
            var executives = await _websiteService.GetTenderNotificationAsync();

            if (executives == null || !executives.Any())
                return Ok(new { message = "No Tender Notifications found.", data = new List<object>() });

            return Ok(executives);
        }

        /// <summary>
        /// Retrieves a list of active Event.
        /// </summary>
        /// <remarks>
        /// This endpoint fetches all active Event from the database.
        /// </remarks>
        /// <response code="200">Successfully retrieved Event. If no records are found, returns an empty list with message: <c>"No Events found."</c></response>
        /// <response code="500">Internal server error.</response>
        [HttpGet("get-event")]
        public async Task<IActionResult> GetEvent()
        {
            var executives = await _websiteService.GetEventAsync();

            if (executives == null || !executives.Any())
                return Ok(new { message = "No Events found.", data = new List<object>() });

            return Ok(executives);
        }

        /// <summary>
        /// Retrieves a list of active Media.
        /// </summary>
        /// <remarks>
        /// This endpoint fetches all active Media from the database.
        /// </remarks>
        /// <response code="200">Successfully retrieved Media. If no records are found, returns an empty list with message: <c>"No Medias found."</c></response>
        /// <response code="500">Internal server error.</response>
        [HttpGet("get-media")]
        public async Task<IActionResult> GetMedia()
        {
            var executives = await _websiteService.GetMediaAsync();

            if (executives == null || !executives.Any())
                return Ok(new { message = "No Medias found.", data = new List<object>() });

            return Ok(executives);
        }

        /// <summary>
        /// Retrieves a list of active LinkedIn.
        /// </summary>
        /// <remarks>
        /// This endpoint fetches all active LinkedIn from the database.
        /// </remarks>
        /// <response code="200">Successfully retrieved LinkedIn. If no records are found, returns an empty list with message: <c>"No LinkedIns found."</c></response>
        /// <response code="500">Internal server error.</response>
        [HttpGet("get-LinkedIn")]
        public async Task<IActionResult> GetLinkedIn()
        {
            var executives = await _websiteService.GetLinkedInAsync();

            if (executives == null || !executives.Any())
                return Ok(new { message = "No LinkedIns found.", data = new List<object>() });

            return Ok(executives);
        }

        /// <summary>
        /// Adds a new email record to the Email Master.
        /// </summary>
        /// <param name="request">The email details to be added.</param>
        /// <remarks>
        /// <para><b>Email Requirements:</b></para>
        /// <para>- <c>Email</c> must be a valid and unique email address.</para>
        /// <para>- <c>IsActive</c> determines whether the email is active.</para>
        /// <para>- <c>AdminId</c> must be a valid admin/user identifier.</para>
        ///
        /// <para><b>Required Fields:</b></para>
        /// <para>- <c>Email</c>: Email address to be added.</para>
        /// <para>- <c>AdminId</c>: ID of the admin performing this action.</para>
        ///
        /// <para><b>Notes:</b></para>
        /// <para>- If an email already exists in the system, the API will return a failure message.</para>
        /// </remarks>
        /// <returns>Returns status code, message, and generated Email ID.</returns>
        /// <response code="200">Email added successfully.</response>
        /// <response code="400">Validation failed — Missing or invalid fields.</response>
        /// <response code="500">An unexpected server error occurred.</response>
        [HttpPost("add-email")]
        public async Task<IActionResult> AddEmail([FromBody] NewEmailMasterRequest request)
        {
            // Manual validation
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

            var (statusCode, message, emailId) = await _emailService.AddEmailAsync(request);

            return StatusCode(statusCode, new { Message = message, EmailId = emailId });
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="citySearch"></param>
        /// <param name="sectorSearch"></param>
        /// <param name="typeSearch"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        [HttpGet("get-incubators")]
        public async Task<IActionResult> GetAll(
        [FromQuery] string? citySearch = null,
        [FromQuery] string? sectorSearch = null,
        [FromQuery] string? typeSearch = null,
        [FromQuery] bool? isActive = null)
        {
            // Call service WITHOUT pagination
            var result = await _incubatorService.GetAllWithoutPaginationAsync(
                citySearch,
                sectorSearch,
                typeSearch,
                isActive   // ← Added filter
            );

            if (result == null || result.Count == 0)
            {
                return Ok(new
                {
                    Message = "No incubators found.",
                    Data = new List<object>()
                });
            }

            return Ok(new
            {
                Message = "Incubator list retrieved successfully.",
                TotalRecords = result.Count,
                Data = result
            });
        }


        [HttpGet("get-sectors")]
        public async Task<IActionResult> GetSectorsForWebsite([FromQuery] bool? isActive = null)
        {
            var sectors = await _sectorService.GetSectorsForWebsiteAsync(isActive);

            if (sectors == null || sectors.Count == 0)
                return Ok(new { message = "No sectors found.", data = new List<object>() });

            return Ok(new
            {
                Status = "Success",
                Data = sectors,
                TotalRecords = sectors.Count
            });
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="isActive"></param>
        /// <returns></returns>
        [HttpGet("get-cities")]
        public async Task<IActionResult> GetCityList([FromQuery] bool? isActive = null)
        {
            var result = await _cityService.GetCityListAsync(isActive);

            if (result == null || result.Data == null || result.Data.Count == 0)
            {
                return Ok(new
                {
                    Message = "No cities found.",
                    Data = new List<object>()
                });
            }

            return Ok(new
            {
                Status = "Success",
                Data = result.Data,
                TotalRecords = result.TotalRecords
            });
        }

        /// <summary>
        /// Retrieves a list of Type Masters by SectorId with optional active filter.
        /// </summary>
        /// <param name="sectorId">Sector Id (required)</param>
        /// <param name="isActive">Active status filter (optional)</param>
        /// <returns>List of TypeMaster with SectorName</returns>
        /// <response code="200">Types fetched successfully.</response>
        /// <response code="400">Invalid SectorId.</response>
        [HttpGet("list-Type")]
        public async Task<IActionResult> GetAllTypes(
        [FromQuery] int sectorId,         // REQUIRED
        [FromQuery] bool? isActive = true  // OPTIONAL
    )
        {
            if (sectorId <= 0)
                return BadRequest(new { Message = "sectorId is required and must be greater than 0." });

            var result = await _websiteService.GetAllTypesAsync(sectorId, isActive);

            if (result == null || result.Count == 0)
                return Ok(new { message = "No records found.", data = new List<object>() });

            return Ok(new
            {
                message = "Types fetched successfully.",
                data = result
            });
        }




        /// <summary>
        ///     
        /// </summary>
        /// <param name="contactId"></param>
        /// <returns></returns>
        [HttpGet("get/{contactId}")]
        public async Task<IActionResult> GetContactById(int contactId)
        {
            var result = await _contactService.GetContactByIdAsync(contactId);

            if (result == null)
                return NotFound(new { message = "Contact not found." });

            return Ok(result);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="designation"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        [HttpGet("get-speakers")]
        public async Task<IActionResult> GetSpeakersNoPagination(
      [FromQuery] string? name,
      [FromQuery] string? designation,
      [FromQuery] bool? isActive)
        {
            var result = await _websiteService.GetAllSpeakersWithoutPaginationAsync(name, designation, isActive);

            if (result == null || result.Count == 0)
                return Ok(new { Message = "No records found.", Data = new List<object>() });

            return Ok(new
            {
                Message = "Speakers retrieved successfully.",
                TotalRecords = result.Count,
                Data = result
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        [HttpGet("list_InitiativeMaster")]
        public async Task<IActionResult> GetInitiativeMaster(
      [FromQuery] string? title,
      [FromQuery] bool? isActive = true)
        {
            var result = await _websiteService.GetInitiativeMasterAsync(title, isActive);

            return Ok(new
            {
                data = result,
                dataCount = result.Count
            });
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="initiativeId"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>

        //    [HttpGet("getall-faqs")]
        //    public async Task<IActionResult> GetFaqs(
        //[FromQuery] int? initiativeId,
        //[FromQuery] bool? isActive)
        //    {
        //        var result = await _websiteService.GetFaqsAsync(initiativeId, isActive);

        //        if (result == null || !result.Any())
        //            return Ok(new { message = "No records found.", data = new List<object>() });

        //        return Ok(new
        //        {
        //            message = "Success",
        //            data = result
        //        });
        //    }


        //[HttpGet("getall-funding-programs")]
        //public async Task<IActionResult> GetFundingPrograms(
        //[FromQuery] string? fundingAgencyName,
        //[FromQuery] bool? isActive)
        //{
        //    var result = await _websiteService.GetFundingProgramsAsync(fundingAgencyName, isActive);

        //    if (result == null || result.Count == 0)
        //        return Ok(new { message = "No records found.", data = new List<object>() });

        //    return Ok(new
        //    {
        //        message = "Funding programs fetched successfully.",
        //        data = result
        //    });
        //}
        /// <returns></returns>
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="initiativeId"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        [HttpGet("getall-photographs")]
        public async Task<IActionResult> GetPhotographs(
           [FromQuery][Required] int? initiativeId,
           [FromQuery] bool? isActive = true)
        {
            // Validate initiativeId
            if (initiativeId == null || initiativeId <= 0)
            {
                return BadRequest(new
                {
                    message = "initiativeId is required and must be greater than 0."
                });
            }

            var result = await _photoService.GetPhotographsAsync(initiativeId.Value, isActive);

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
                message = "Photographs fetched successfully.",
                data = result
            });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="initiativeId"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>

        [HttpGet("getall-testimonials")]
        public async Task<IActionResult> GetTestimonials(
    [FromQuery][Required] int? initiativeId,
    [FromQuery] bool? isActive = true)
        {
            // Validate initiativeId
            if (initiativeId == null || initiativeId <= 0)
            {
                return BadRequest(new
                {
                    message = "initiativeId is required and must be greater than 0."
                });
            }

            var result = await _service.GetTestimonialsAsync(initiativeId.Value, isActive);

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
                message = "Testimonials fetched successfully.",
                data = result
            });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sectorId"></param>
        /// <param name="isActive"></param>
        /// <param name="initiativeId"></param>
        /// <returns></returns>
        [HttpGet("getall-winners")]
        public async Task<IActionResult> GetWinners(
     [FromQuery] int? sectorId,
     [FromQuery] int? initiativeId,
     [FromQuery] bool? isActive = true)   // OPTIONAL PARAM LAST ✔
        {
            var result = await _winnerService.GetWinnersAsync(sectorId, isActive, initiativeId);

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
                message = "Winners fetched successfully.",
                data = result
            });
        }

        /// <summary>
        /// </summary>
        /// <param name="isActive"></param>
        /// <param name="initiativeId"></param>
        /// <returns></returns>
        [HttpGet("getall-keyfactors")]
        public async Task<IActionResult> GetKeyFactors(
     [FromQuery] bool? isActive = true,
     [FromQuery] int? initiativeId = null)
        {
            var result = await _keyFactorService.GetKeyFactorsAsync(isActive, initiativeId);

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
                message = "Key factors fetched successfully.",
                data = result
            });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="isActive"></param>
        /// <param name="initiativeId"></param>
        /// <returns></returns>

        [HttpGet("getall-sector-opprtunity")]
        public async Task<IActionResult> GetSectorOpprtunity(
            [FromQuery] bool? isActive = true,
            [FromQuery] int? initiativeId = null)
        {
            var result = await _sectorOpprtunityService
                .GetSectorOpprtunityAsync(isActive, initiativeId);

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
                message = "Sector opportunities fetched successfully.",
                data = result
            });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="isActive"></param>
        /// <param name="initiativeId"></param>
        /// <returns></returns>
        [HttpGet("getall-partners")]
        public async Task<IActionResult> GetPartners(
            [FromQuery] bool? isActive = true,
            [FromQuery] int? initiativeId = null)
        {
            // Correct parameter order: initiativeId first, then isActive
            var result = await _partnersService.GetPartnersAsync(initiativeId, isActive);

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
                message = "Partners fetched successfully.",
                data = result
            });
        }
    }
}

