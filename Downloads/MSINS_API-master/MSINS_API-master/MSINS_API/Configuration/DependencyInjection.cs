using MSINS_API.Exceptions.Handler;
using MSINS_API.Services;
using JobPortalAPI.Services;
using MSINS_API.Repositories.Implementation;
using MSINS_API.Repositories.Interface;
using MSINS_API.Services.Implementation;
using MSINS_API.Services.Interface;

namespace MSINS_API.Configuration
{
    public static class DependencyInjection
    {
        public static void AddCustomServices(this IServiceCollection Services)
        {
            // Register Interfaces

            Services.AddScoped<ISuggestionRepository, SuggestionRepository>();
            Services.AddScoped<IFeedbackRepository, FeedbackRepository>();
            Services.AddScoped<IGrievanceRepository, GrievanceRepository>();
            Services.AddScoped<IPublicConsultationRepository, PublicConsultationRepository>();
            Services.AddScoped<IPublicConsultationListRepository, PublicConsultationListRepository>();
            Services.AddScoped<IBannerRepository, BannerRepository>();
            Services.AddScoped<IEcoSystemRepository, EcoSystemRepository>();
            Services.AddScoped<IEcoSystemRepository, EcoSystemRepository>();
            Services.AddScoped<IPartnerRepository, PartnerRepository>();
            Services.AddScoped<IAboutUsRepository, AboutUsRepository>();
            Services.AddScoped<IUtilityRepository, UtilityRepository>();
            Services.AddScoped<ILeadershipRepository, LeadershipRepository>();
            Services.AddScoped<IInitiativeRepository, InitiativeRepository>();
            Services.AddScoped<IExecutiveRepository, ExecutiveRepository>();
            Services.AddScoped<IGoverningCouncilRepository, GoverningCouncilRepository>();
            Services.AddScoped<IGeneralBodyRepository, GeneralBodyRepository>();
            Services.AddScoped<IVideoRepository, VideoRepository>();
            Services.AddScoped<IFeaturedResourceRepository, FeaturedResourceRepository>();
            Services.AddScoped<IGovernmentResolutionRepository, GovernmentResolutionRepository>();
            Services.AddScoped<ITenderNotificationRepository, TenderNotificationRepository>();
            Services.AddScoped<IEventRepository, EventRepository>();
            Services.AddScoped<IMediaRepository, MediaRepository>();
            Services.AddScoped<ILinkedInRepository, LinkedInRepository>();
            Services.AddScoped<INewSpeakersMasterRepository, NewSpeakersMasterRepository>();
            Services.AddScoped<INewSectorMasterRepository, NewSectorMasterRepository>();
            Services.AddScoped<INewTypeMasterRepository, NewTypeMasterRepository>();
            Services.AddScoped<INewEmailMasterRepository, NewEmailMasterRepository>();
            Services.AddScoped<INewIncubatorsMasterRepository, NewIncubatorsMasterRepository>();
            Services.AddScoped<INewCityMasterRepository, NewCityMasterRepository>();
            Services.AddScoped<INewContactMasterService, NewContactMasterService>();
            Services.AddScoped<INewInitiativeSpeakerService, NewInitiativeSpeakerService>();
            Services.AddScoped<INewInitiativeMasterRepository, NewInitiativeMasterRepository>();
            Services.AddScoped<INewFaqsRepository, NewFaqsRepository>();
            Services.AddScoped<INewFundingProgramsRepository, NewFundingProgramsRepository>();
            Services.AddScoped<INewTestimonialsMasterRepository, NewTestimonialsMasterRepository>();
            Services.AddScoped<INewPhotographsMasterRepository, NewPhotographsMasterRepository>();
            Services.AddScoped<INewWinnerMasterRepository, NewWinnerMasterRepository>();
            Services.AddScoped<INewKeyFactorMasterRepository, NewKeyFactorMasterRepository>();
            Services.AddScoped<INewSectorOpprtunityMasterRepository, NewSectorOpprtunityMasterRepository>();
            Services.AddScoped<INewInitiativePartnersMasterRepository, NewInitiativePartnersMasterRepository>();

            // Register Services


            Services.AddScoped<IHashGenValidate, HashGenValidate>();
            Services.AddScoped<ISuggestionService, SuggestionService>();
            Services.AddScoped<IEmailService, EmailService>();
            Services.AddScoped<IFeedbackService, FeedbackService>();
            Services.AddScoped<IGrievanceService, GrievanceService>();
            Services.AddScoped<IPublicConsultationService, PublicConsultationService>();
            Services.AddScoped<IFileUploadService, FileUploadService>();
            Services.AddScoped<IPublicConsultationListService, PublicConsultationListService>();
            Services.AddScoped<IFileUploadCustomSizeService, FileUploadCustomSizeService>();
            Services.AddScoped<IBannerService, BannerService>();
            Services.AddScoped<IEcoSystemService, EcoSystemService>();
            Services.AddScoped<IPartnerService, PartnerService>();
            Services.AddScoped<IAboutUsService, AboutService>();
            Services.AddScoped<IWebsiteService, WebsiteService>();
            Services.AddScoped<ILeadershipService, LeadershipService>();
            Services.AddScoped<IInitiativeService, InitiativeService>();
            Services.AddScoped<IExecutiveService, ExecutiveService>();
            Services.AddScoped<IGoverningCouncilService, GoverningCouncilService>();
            Services.AddScoped<IGeneralBodyService, GeneralBodyService>();
            Services.AddScoped<IVideoService, VideoService>();
            Services.AddScoped<IFeaturedResourceService, FeaturedResourceService>();
            Services.AddScoped<IGovernmentResolutionService, GovernmentResolutionService>();
            Services.AddScoped<ITenderNotificationService, TenderNotificationService>();
            Services.AddScoped<IEventService, EventService>();
            Services.AddScoped<IMediaService, MediaService>();
            Services.AddScoped<ILinkedInService, LinkedInService>();
            Services.AddScoped<INewSpeakersMasterService, NewSpeakersMasterService>();
            Services.AddScoped<INewSectorMasterService, NewSectorMasterService>();
            Services.AddScoped<INewTypeMasterService, NewTypeMasterService>();
            Services.AddScoped<INewEmailMasterService, NewEmailMasterService>();
            Services.AddScoped<INewIncubatorsMasterService, NewIncubatorsMasterService>();
            Services.AddScoped<IFileUploadCustomSizeService, FileUploadCustomSizeService>();
            //Services.AddScoped<IFileUploadCustomSizeService, FileUploadCustomSizeService>();
            Services.AddScoped<INewCityMasterService, NewCityMasterService>();
            Services.AddScoped<INewContactMasterRepository, NewContactMasterRepository>();
            Services.AddScoped<INewInitiativeSpeakerRepository, NewInitiativeSpeakerRepository>();
            Services.AddScoped<IFileUploadCustomSizeService, FileUploadCustomSizeService>();
            Services.AddScoped<INewInitiativeMasterService, NewInitiativeMasterService>();
            Services.AddScoped<IFileUploadCustomSizeService, FileUploadCustomSizeService>();
            Services.AddScoped<INewFaqsService, NewFaqsService>();
            Services.AddScoped<INewFundingProgramsService, NewFundingProgramsService>();
            Services.AddScoped<INewTestimonialsMasterService, NewTestimonialsMasterService>();
            Services.AddScoped<INewPhotographsMasterService, NewPhotographsMasterService>();
            Services.AddScoped<INewWinnerMasterService, NewWinnerMasterService>();
            Services.AddScoped<INewKeyFactorMasterService, NewKeyFactorMasterService>();
            Services.AddScoped<INewSectorOpprtunityMasterService, NewSectorOpprtunityMasterService>();
            Services.AddScoped<INewInitiativePartnersMasterService, NewInitiativePartnersMasterService>();
        }

        public static void AddCustomeExceptionHandler(this IServiceCollection Services)
        {
            // add exception handler
            Services.AddExceptionHandler<ValidationExceptionHandler>();
            Services.AddExceptionHandler<NotFoundExceptionHandler>();
            Services.AddExceptionHandler<SQLExceptionHandler>();
            Services.AddExceptionHandler<InternalExceptionHandler>();
            Services.AddExceptionHandler<SmtpExceptionHandler>();
            Services.AddExceptionHandler<GlobalExceptionHandler>();
            Services.AddProblemDetails();
        }
    }
}
