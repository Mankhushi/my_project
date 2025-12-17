using MSINS_API.Models.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MSINS_API.Services.Interface
{
    public interface IWebsiteService
    {
        Task<List<BannerResponse>> GetActiveBanners();
        Task<List<AboutUsResponse>> GetAboutUsDetailsAsync();
        Task<List<EcoSystemResponse>> GetEcoSystemDetailsAsync();
        Task<List<PartnersResponse>> GetPartnersAsync();
        Task<List<LeadershipResponse>> GetLeadersAsync();
        Task<List<InitiativeResponse>> GetInitiativeAsync();
        Task<List<ExecutiveResponse>> GetExecutiveAsync();
        Task<List<GoverningCouncilResponse>> GetCouncilAsync();
        Task<List<GeneralBodyResponse>> GetgeneralBodyAsync();
        Task<List<VideoResponse>> GetFeaturedVideoAsync();
        Task<List<VideoResponse>> GetLatestVideoAsync();
        Task<List<FeaturedResourceResponse>> GetFeaturedResourceAsync();
        Task<List<GovernmentResolutionResponse>> GetGovernmentResolutionAsync();
        Task<List<TenderNotificationResponse>> GetTenderNotificationAsync();
        Task<List<EventResponse>> GetEventAsync();
        Task<List<MediaResponse>> GetMediaAsync();
        Task<List<LinkedInResponse>> GetLinkedInAsync();
        Task<List<NewInitiativeSpeakerResponse>> GetAllSpeakersWithoutPaginationAsync(string? name, string? designation, bool? isActive);
        Task<List<NewInitiativeMasterResponse>> GetInitiativeMasterAsync(string? title, bool? isActive);
        Task<List<NewFaqsResponse>> GetFaqsAsync(int? initiativeId, bool? isActive);
        //Task GetFundingProgramsAsync(string? fundingAgencyName, bool? isActive);
        //Task<List<NewFundingProgramsResponse>> GetFundingProgramsAsync(string? fundingAgencyName, bool? isActive);
        Task<List<NewTypeMasterResponse>> GetAllTypesAsync(int sectorId, bool? isActive);

        Task<List<NewIncubatorsMasterResponse>> GetAllWithoutPaginationAsync(
int? incubatorId,
int? cityId,
int? sectorId,
int? typeId,
bool? isActive);
    }
}

