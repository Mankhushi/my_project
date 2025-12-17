using Microsoft.Data.SqlClient;
using MSINS_API.Models.Request;
using MSINS_API.Models.Response;
using MSINS_API.POCO;
using MSINS_API.Repositories.Implementation;
using MSINS_API.Repositories.Interface;
using MSINS_API.Services.Interface;
using System.Data;

namespace MSINS_API.Services.Implementation
{

    public class WebsiteService : IWebsiteService
    {
        private readonly IUtilityRepository _utilityRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly BaseUrlSettings _baseUrlSettings;
        private readonly INewInitiativeSpeakerRepository _speakerRepository;
        private readonly INewFaqsRepository _FaqsRepository;
        private readonly INewFundingProgramsRepository _FundingProgramsRepository;





        public WebsiteService(IUtilityRepository utilityRepository, IHttpContextAccessor httpContextAccessor, BaseUrlSettings baseUrlSettings, INewInitiativeSpeakerRepository speakerRepository, INewFaqsRepository FaqsRepository, INewFundingProgramsRepository FundingProgramsRepository)
        {
            _utilityRepository = utilityRepository;
            _httpContextAccessor = httpContextAccessor;
            _baseUrlSettings = baseUrlSettings;
            _speakerRepository = speakerRepository;
            _FaqsRepository = FaqsRepository;
            _FundingProgramsRepository = FundingProgramsRepository;

        }

        public async Task<List<BannerResponse>> GetActiveBanners()
        {
            string tableName = "Banners"; // Table Name
            string columns = "BannerType, BannerName, BannerLink, LinkType, ImagePath, IsActive"; // Columns to Fetch
            string orderColumn = "BannerId"; // Ordering by Name
            string orderDirection = "ASC"; // Sorting Order

            var data = await _utilityRepository.GetTableDataAsync(tableName, columns, orderColumn, orderDirection, true);

            // Get Base URL dynamically from HttpContext
            var request = _httpContextAccessor.HttpContext?.Request;
            string baseUrl = request != null
                ? $"{request.Scheme}://{request.Host}"
                : _baseUrlSettings.Production; // Fallback to Production URL from settings

            var banners = data.Select(row => new BannerResponse
            {
                Type = row["BannerType"].ToString(),
                Name = row["BannerName"].ToString(),
                Link = row["BannerLink"]?.ToString(),
                LinkType = row["LinkType"] != DBNull.Value ? (Convert.ToBoolean(row["LinkType"]) ? "Internal" : "External") : null,
                ImagePath = !string.IsNullOrEmpty(row["ImagePath"]?.ToString())
                ? $"{baseUrl}/{row["ImagePath"].ToString().TrimStart('/')}"
                : null,
                isActive = Convert.ToBoolean(row["IsActive"]) ? "Active" : "Inactive"
            }).ToList();

            return banners;
        }

        public async Task<List<AboutUsResponse>> GetAboutUsDetailsAsync()
        {
            string tableName = "AboutUs"; // Table Name
            string columns = "TextOne, TextTwo, ImagePath, PDFPath"; // Columns to Fetch
            string orderColumn = "AboutUsId"; // Ordering by ID
            string orderDirection = "ASC"; // Sorting Order

            var data = await _utilityRepository.GetTableDataAsync(tableName, columns, orderColumn, orderDirection, true);

            // Get Base URL dynamically from HttpContext
            var request = _httpContextAccessor.HttpContext?.Request;
            string baseUrl = request != null
                ? $"{request.Scheme}://{request.Host}"
                : _baseUrlSettings.Production; // Fallback to Production URL from settings

            var aboutUsList = data.Select(row => new AboutUsResponse
            {
                TextOne = row["TextOne"].ToString(),
                TextTwo = row["TextTwo"].ToString(),
                ImageFile = !string.IsNullOrEmpty(row["ImagePath"]?.ToString())
                    ? $"{baseUrl}/{row["ImagePath"].ToString().TrimStart('/')}"
                    : null,
                PDFFile = !string.IsNullOrEmpty(row["PDFPath"]?.ToString())
                    ? $"{baseUrl}/{row["PDFPath"].ToString().TrimStart('/')}"
                    : null
            }).ToList();

            return aboutUsList;
        }

        public async Task<List<EcoSystemResponse>> GetEcoSystemDetailsAsync()
        {
            string tableName = "EcoSystem"; // Table Name
            string columns = "TextOne, TextTwo, ImagePath, IsActive"; // Columns to Fetch
            string orderColumn = "EcoSystemId"; // Ordering by ID
            string orderDirection = "ASC"; // Sorting Order

            var data = await _utilityRepository.GetTableDataAsync(tableName, columns, orderColumn, orderDirection, true);

            // Get Base URL dynamically from HttpContext
            var request = _httpContextAccessor.HttpContext?.Request;
            string baseUrl = request != null
                ? $"{request.Scheme}://{request.Host}"
                : _baseUrlSettings.Production; // Fallback to Production URL from settings

            var ecoSystemList = data.Select(row => new EcoSystemResponse
            {
                TextOne = row["TextOne"].ToString(),
                TextTwo = row["TextTwo"].ToString(),
                ImagePath = !string.IsNullOrEmpty(row["ImagePath"]?.ToString())
                    ? $"{baseUrl}/{row["ImagePath"].ToString().TrimStart('/')}"
                    : null,
                isActive = Convert.ToBoolean(row["IsActive"]) ? "Active" : "Inactive"
            }).ToList();

            return ecoSystemList;
        }

        public async Task<List<PartnersResponse>> GetPartnersAsync()
        {
            string tableName = "Partners"; // Table Name
            string columns = "PartnerName, PartnerLink, LinkType, imagePath, isActive"; // Columns to Fetch
            string orderColumn = "PartnerId"; // Ordering by Name
            string orderDirection = "ASC"; // Sorting Order

            var data = await _utilityRepository.GetTableDataAsync(tableName, columns, orderColumn, orderDirection, true);

            // Get Base URL dynamically from HttpContext
            var request = _httpContextAccessor.HttpContext?.Request;
            string baseUrl = request != null
                ? $"{request.Scheme}://{request.Host}"
                : _baseUrlSettings.Production; // Fallback to Production URL from settings

            var partners = data.Select(row => new PartnersResponse
            {
                Name = row["PartnerName"].ToString(),
                Link = row["PartnerLink"]?.ToString(),
                LinkType = row["LinkType"] != DBNull.Value ? (Convert.ToBoolean(row["LinkType"]) ? "Internal" : "External") : null,
                ImagePath = !string.IsNullOrEmpty(row["imagePath"]?.ToString())
                    ? $"{baseUrl}/{row["imagePath"].ToString().TrimStart('/')}"
                    : null,
                isActive = Convert.ToBoolean(row["isActive"]) ? "Active" : "Inactive"
            }).ToList();

            return partners;
        }

        public async Task<List<LeadershipResponse>> GetLeadersAsync()
        {
            string tableName = "Leadership"; // Table Name
            string columns = "LeaderName, Designation, imagePath, isActive"; // Columns to Fetch
            string orderColumn = "LeadershipId"; // Ordering by Name
            string orderDirection = "ASC"; // Sorting Order

            var data = await _utilityRepository.GetTableDataAsync(tableName, columns, orderColumn, orderDirection, true);

            // Get Base URL dynamically from HttpContext
            var request = _httpContextAccessor.HttpContext?.Request;
            string baseUrl = request != null
                ? $"{request.Scheme}://{request.Host}"
                : _baseUrlSettings.Production; // Fallback to Production URL from settings

            var leaders = data.Select(row => new LeadershipResponse
            {
                LeaderName = row["LeaderName"].ToString(),
                Designation = row["Designation"]?.ToString(),
                ImagePath = !string.IsNullOrEmpty(row["imagePath"]?.ToString())
                    ? $"{baseUrl}/{row["imagePath"].ToString().TrimStart('/')}"
                    : null,
                isActive = Convert.ToBoolean(row["isActive"]) ? "Active" : "Inactive"
            }).ToList();

            return leaders;
        }

        public async Task<List<InitiativeResponse>> GetInitiativeAsync()
        {
            string tableName = "Initiative"; // Table Name
            string columns = "InitiativeName, initiativeLink,InitiativeDesc, linkType, imagePath, isActive"; // Columns to Fetch
            string orderColumn = "InitiativeId"; // Ordering by Name
            string orderDirection = "ASC"; // Sorting Order

            var data = await _utilityRepository.GetTableDataAsync(tableName, columns, orderColumn, orderDirection, true);

            // Get Base URL dynamically from HttpContext
            var request = _httpContextAccessor.HttpContext?.Request;
            string baseUrl = request != null
                ? $"{request.Scheme}://{request.Host}"
                : _baseUrlSettings.Production; // Fallback to Production URL from settings

            var leaders = data.Select(row => new InitiativeResponse
            {
                InitiativeName = row["InitiativeName"].ToString(),
                Link = row["initiativeLink"]?.ToString(),
                LinkType = row["linkType"]?.ToString(),
                Description = row["InitiativeDesc"]?.ToString(),
                ImagePath = !string.IsNullOrEmpty(row["imagePath"]?.ToString())
                    ? $"{baseUrl}/{row["imagePath"].ToString().TrimStart('/')}"
                    : null,
                isActive = Convert.ToBoolean(row["isActive"]) ? "Active" : "Inactive"
            }).ToList();

            return leaders;
        }

        public async Task<List<ExecutiveResponse>> GetExecutiveAsync()
        {
            string tableName = "Executive"; // Table Name
            string columns = "ExecutiveName, ExecutiveDesc, imagePath, isActive"; // Columns to Fetch
            string orderColumn = "ExecutiveId"; // Ordering by Name
            string orderDirection = "ASC"; // Sorting Order

            var data = await _utilityRepository.GetTableDataAsync(tableName, columns, orderColumn, orderDirection, true);

            // Get Base URL dynamically from HttpContext
            var request = _httpContextAccessor.HttpContext?.Request;
            string baseUrl = request != null
                ? $"{request.Scheme}://{request.Host}"
                : _baseUrlSettings.Production; // Fallback to Production URL from settings

            var executive = data.Select(row => new ExecutiveResponse
            {
                Name = row["ExecutiveName"].ToString(),
                Description = row["ExecutiveDesc"]?.ToString(),
                ImagePath = !string.IsNullOrEmpty(row["imagePath"]?.ToString())
                    ? $"{baseUrl}/{row["imagePath"].ToString().TrimStart('/')}"
                    : null,
                IsActive = Convert.ToBoolean(row["isActive"]) ? "Active" : "Inactive"
            }).ToList();

            return executive;
        }

        public async Task<List<GoverningCouncilResponse>> GetCouncilAsync()
        {
            string tableName = "GoverningCouncil"; // Table Name
            string columns = "Name, Position, Designation,Address, isActive"; // Columns to Fetch
            string orderColumn = "GoverningCouncilId"; // Ordering by Name
            string orderDirection = "ASC"; // Sorting Order

            var data = await _utilityRepository.GetTableDataAsync(tableName, columns, orderColumn, orderDirection, true);


            var council = data.Select(row => new GoverningCouncilResponse
            {
                Name = row["Name"].ToString(),
                Position = row["Position"]?.ToString(),
                Designation = row["Designation"]?.ToString(),
                Address = row["Address"]?.ToString(),
                IsActive = Convert.ToBoolean(row["isActive"]) ? "Active" : "Inactive"
            }).ToList();

            return council;
        }

        public async Task<List<GeneralBodyResponse>> GetgeneralBodyAsync()
        {
            string tableName = "GeneralBody"; // Table Name
            string columns = "Name, Position, Designation,Address, isActive"; // Columns to Fetch
            string orderColumn = "GeneralBodyId"; // Ordering by Name
            string orderDirection = "ASC"; // Sorting Order

            var data = await _utilityRepository.GetTableDataAsync(tableName, columns, orderColumn, orderDirection, true);


            var general = data.Select(row => new GeneralBodyResponse
            {
                Name = row["Name"].ToString(),
                Position = row["Position"]?.ToString(),
                Designation = row["Designation"]?.ToString(),
                Address = row["Address"]?.ToString(),
                IsActive = Convert.ToBoolean(row["isActive"]) ? "Active" : "Inactive"
            }).ToList();

            return general;
        }

        public async Task<List<VideoResponse>> GetFeaturedVideoAsync()
        {
            string tableName = "Video"; // Table Name
            string columns = "Title, Link"; // Columns to Fetch
            string orderColumn = "VideoId"; // Ordering by GeneralBodyId
            string orderDirection = "ASC"; // Sorting Order

            // Filtering conditions
            string conditionBoolColumn = "isActive";
            int conditionBoolValue = 1; // Only active records

            string conditionBoolColumn1 = "isFeatured";
            int conditionBoolValue1 = 1; // Only featured records (1 = true)

            var data = await _utilityRepository.GetFilteredTableDataAsync(
                tableName, columns, orderColumn, orderDirection,
                 null, null, // No string condition
                null, null, // No string condition
                conditionBoolColumn, conditionBoolValue, // Filter for isActive = 1
                conditionBoolColumn1, conditionBoolValue1 // Filter for isActive = 1
            );

            var general = data.Select(row => new VideoResponse
            {
                Title = row["Title"].ToString(),
                Link = row["Link"]?.ToString()
            }).ToList();

            return general;
        }

        public async Task<List<VideoResponse>> GetLatestVideoAsync()
        {
            string tableName = "Video"; // Table Name
            string columns = "Title, Link"; // Columns to Fetch
            string orderColumn = "VideoId"; // Ordering by GeneralBodyId
            string orderDirection = "ASC"; // Sorting Order

            // Filtering conditions
            string conditionBoolColumn = "isActive";
            int conditionBoolValue = 1; // Only active records

            string conditionBoolColumn1 = "isLatest";
            int conditionBoolValue1 = 1; // Only featured records (1 = true)

            var data = await _utilityRepository.GetFilteredTableDataAsync(
                tableName, columns, orderColumn, orderDirection,
                 null, null, // No string condition
                null, null, // No string condition
                conditionBoolColumn, conditionBoolValue, // Filter for isActive = 1
                conditionBoolColumn1, conditionBoolValue1 // Filter for isActive = 1
            );

            var general = data.Select(row => new VideoResponse
            {
                Title = row["Title"].ToString(),
                Link = row["Link"]?.ToString()
            }).ToList();

            return general;
        }

        public async Task<List<FeaturedResourceResponse>> GetFeaturedResourceAsync()
        {
            string tableName = "FeaturedResource"; // Table Name
            string columns = "Title, FeaturedResourceDate, Link"; // Columns to Fetch
            string orderColumn = "FeaturedResourceId"; // Ordering by Name
            string orderDirection = "ASC"; // Sorting Order

            var data = await _utilityRepository.GetTableDataAsync(tableName, columns, orderColumn, orderDirection, true);

            // Get Base URL dynamically from HttpContext
            var request = _httpContextAccessor.HttpContext?.Request;
            string baseUrl = request != null
                ? $"{request.Scheme}://{request.Host}"
                : _baseUrlSettings.Production;       // Production URL from settings

            var council = data.Select(row => new FeaturedResourceResponse
            {
                Title = row["Title"].ToString(),
                FeaturedResourceDate = row["FeaturedResourceDate"] != DBNull.Value
                ? Convert.ToDateTime(row["FeaturedResourceDate"]).ToString("yyyy-MM-dd")  // Convert & format date
                : null,
                Link = $"{baseUrl}/{row["Link"].ToString().TrimStart('/')}"
            }).ToList();

            return council;
        }

        public async Task<List<GovernmentResolutionResponse>> GetGovernmentResolutionAsync()
        {
            string tableName = "GovernmentResolution"; // Table Name
            string columns = "Title,  Link"; // Columns to Fetch
            string orderColumn = "GovernmentResolutionId"; // Ordering by Name
            string orderDirection = "ASC"; // Sorting Order

            var data = await _utilityRepository.GetTableDataAsync(tableName, columns, orderColumn, orderDirection, true);

            // Get Base URL dynamically from HttpContext
            var request = _httpContextAccessor.HttpContext?.Request;
            string baseUrl = request != null
                ? $"{request.Scheme}://{request.Host}"
                : _baseUrlSettings.Production;       // Production URL from settings

            var council = data.Select(row => new GovernmentResolutionResponse
            {
                Title = row["Title"].ToString(),
                Link = $"{baseUrl}/{row["Link"].ToString().TrimStart('/')}"
            }).ToList();

            return council;
        }

        public async Task<List<TenderNotificationResponse>> GetTenderNotificationAsync()
        {
            string tableName = "TenderNotification"; // Table Name
            string columns = "*"; // Columns to Fetch
            string orderColumn = "TenderNotificationId"; // Ordering by Name
            string orderDirection = "ASC"; // Sorting Order

            var data = await _utilityRepository.GetTableDataAsync(tableName, columns, orderColumn, orderDirection, true);

            // Get Base URL dynamically from HttpContext
            var request = _httpContextAccessor.HttpContext?.Request;
            string baseUrl = request != null
                ? $"{request.Scheme}://{request.Host}"
                : _baseUrlSettings.Production;       // Production URL from settings

            var council = data.Select(row => new TenderNotificationResponse
            {
                Title = row["Title"].ToString(),
                TenderId = row["TenderId"].ToString(),
                RefNo = row["RefNo"].ToString(),
                PublishedDate = row["PublishedDate"] != DBNull.Value
                ? Convert.ToDateTime(row["PublishedDate"]).ToString("yyyy-MM-dd HH:mm:ss")  // Convert & format date
                : null,
                Category = row["Category"].ToString(),
                Status = row["Status"].ToString()

            }).ToList();

            return council;
        }

        public async Task<List<EventResponse>> GetEventAsync()
        {
            string tableName = "Event"; // Table Name
            string columns = "*"; // Columns to Fetch
            string orderColumn = "EventId"; // Ordering by Name
            string orderDirection = "ASC"; // Sorting Order

            var data = await _utilityRepository.GetTableDataAsync(tableName, columns, orderColumn, orderDirection, true);

            // Get Base URL dynamically from HttpContext
            var request = _httpContextAccessor.HttpContext?.Request;
            string baseUrl = request != null
                ? $"{request.Scheme}://{request.Host}"
                : _baseUrlSettings.Production;       // Production URL from settings

            var council = data.Select(row => new EventResponse
            {
                EventName = row["EventName"].ToString(),
                EventLocation = row["EventLocation"].ToString(),
                EventType = row["EventType"].ToString(),
                EventDate = row["EventDate"] != DBNull.Value
                ? Convert.ToDateTime(row["EventDate"]).ToString("yyyy-MM-dd")  // Convert & format date
                : null,
                EventDesc = row["EventDesc"].ToString(),
                ImagePath = !string.IsNullOrEmpty(row["ImagePath"]?.ToString())
                ? $"{baseUrl}/{row["ImagePath"].ToString().TrimStart('/')}"
                : null

            }).ToList();

            return council;
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public async Task<List<MediaResponse>> GetMediaAsync()
        {
            string tableName = "Media"; // Table Name
            string columns = "*"; // Columns to Fetch
            string orderColumn = "MediaId"; // Ordering by Name
            string orderDirection = "ASC"; // Sorting Order

            var data = await _utilityRepository.GetTableDataAsync(tableName, columns, orderColumn, orderDirection, true);

            // Get Base URL dynamically from HttpContext
            var request = _httpContextAccessor.HttpContext?.Request;
            string baseUrl = request != null
                ? $"{request.Scheme}://{request.Host}"
                : _baseUrlSettings.Production;       // Production URL from settings

            var council = data.Select(row => new MediaResponse
            {
                MediaName = row["MediaName"].ToString(),
                Link = row["MediaLink"].ToString(),
                MediaDate = row["MediaDate"] != DBNull.Value
                ? Convert.ToDateTime(row["MediaDate"]).ToString("yyyy-MM-dd")  // Convert & format date
                : null,
                MediaDesc = row["MediaDesc"].ToString(),
                ImagePath = !string.IsNullOrEmpty(row["ImagePath"]?.ToString())
                ? $"{baseUrl}/{row["ImagePath"].ToString().TrimStart('/')}"
                : null

            }).ToList();

            return council;
        }

        //------------------------------------ GET ALL LINKESD---------------------------------
        public async Task<List<LinkedInResponse>> GetLinkedInAsync()
        {
            string tableName = "LinkedIn"; // Table Name
            string columns = "*"; // Columns to Fetch
            string orderColumn = "LinkedInId"; // Ordering by Name
            string orderDirection = "ASC"; // Sorting Order

            var data = await _utilityRepository.GetTableDataAsync(tableName, columns, orderColumn, orderDirection, true);

            // Get Base URL dynamically from HttpContext
            var request = _httpContextAccessor.HttpContext?.Request;
            string baseUrl = request != null
                ? $"{request.Scheme}://{request.Host}"
                : _baseUrlSettings.Production;       // Production URL from settings

            var council = data.Select(row => new LinkedInResponse
            {
                Title = row["Title"].ToString(),
                Link = row["Link"].ToString()

            }).ToList();

            return council;
        }

        //-------------------------GET ALL SPEAKER----------------------------
        public async Task<List<NewInitiativeSpeakerResponse>> GetAllSpeakersWithoutPaginationAsync(string? name, string? designation, bool? isActive)
        {
            return await _utilityRepository.GetAllSpeakersWithoutPaginationAsync(
                name,
                designation,
                isActive
            );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        public async Task<List<NewInitiativeMasterResponse>> GetInitiativeMasterAsync(string? title, bool? isActive)
        {
            return await _utilityRepository.GetInitiativeMasterAsync(title, isActive);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="initiativeId"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        public async Task<List<NewFaqsResponse>> GetFaqsAsync(int? initiativeId, bool? isActive)
        {
            return await _utilityRepository.GetFaqsAsync(initiativeId, isActive);
        }


        //public async Task<List<NewFundingProgramsResponse>> GetFundingProgramsAsync(string? fundingAgencyName, bool? isActive)
        //{
        //    return await _utilityRepository.GetFundingProgramsAsync(fundingAgencyName, isActive);
        //}


        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        public async Task<List<NewTypeMasterResponse>> GetAllTypesAsync(int sectorId, bool? isActive)
        {
            return await _utilityRepository.GetAllTypesAsync(sectorId, isActive);
        }


        public async Task<List<NewIncubatorsMasterResponse>> GetAllWithoutPaginationAsync(
        int? incubatorId,
        int? cityId,
        int? sectorId,
        int? typeId,
        bool? isActive)
        {
            return await _utilityRepository.GetAllWithoutPaginationAsync(
                incubatorId,
                cityId,
                sectorId,
                typeId,
                isActive ?? true);   // ⭐ default active
        }
    }
}

