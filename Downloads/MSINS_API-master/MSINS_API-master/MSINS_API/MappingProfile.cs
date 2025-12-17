using AutoMapper;
using MSINS_API.Models.Request;
using MSINS_API.Models.Response;
using System.Data;

namespace JobPortalAPI
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<IDataRecord, PublicConsultationListResponse>()
                // For required properties (non-nullable), provide a default (empty string) if DBNull.
                .ForMember(dest => dest.PublicConsultationId,
                    opt => opt.MapFrom(src => src["PublicConsultationId"] == DBNull.Value ? string.Empty : src["PublicConsultationId"].ToString()))
                .ForMember(dest => dest.FullName,
                    opt => opt.MapFrom(src => src["FullName"] == DBNull.Value ? string.Empty : src["FullName"].ToString()))
                .ForMember(dest => dest.Email,
                    opt => opt.MapFrom(src => src["Email"] == DBNull.Value ? string.Empty : src["Email"].ToString()))
                .ForMember(dest => dest.ContactNumber,
                    opt => opt.MapFrom(src => src["ContactNumber"] == DBNull.Value ? string.Empty : src["ContactNumber"].ToString()))
                // For nullable properties, allow null if DBNull.
                .ForMember(dest => dest.CityDistrict,
                    opt => opt.MapFrom(src => src["CityDistrict"] == DBNull.Value ? null : src["CityDistrict"].ToString()))
                .ForMember(dest => dest.OrganizationName,
                    opt => opt.MapFrom(src => src["OrganizationName"] == DBNull.Value ? null : src["OrganizationName"].ToString()))
                .ForMember(dest => dest.ExpertiseSector,
                    opt => opt.MapFrom(src => src["ExpertiseSector"] == DBNull.Value ? null : src["ExpertiseSector"].ToString()))
                .ForMember(dest => dest.ExpertiseSectorOther,
                    opt => opt.MapFrom(src => src["ExpertiseSectorOther"] == DBNull.Value ? null : src["ExpertiseSectorOther"].ToString()))
                .ForMember(dest => dest.AspectPolicy,
                    opt => opt.MapFrom(src => src["AspectPolicy"] == DBNull.Value ? null : src["AspectPolicy"].ToString()))
                .ForMember(dest => dest.Suggestion,
                    opt => opt.MapFrom(src => src["Suggestion"] == DBNull.Value ? null : src["Suggestion"].ToString()))
                .ForMember(dest => dest.Rating,
                    opt => opt.MapFrom(src => src["Rating"] == DBNull.Value ? null : src["Rating"].ToString()))
                .ForMember(dest => dest.RecommendateProgram,
                    opt => opt.MapFrom(src => src["RecommendateProgram"] == DBNull.Value ? null : src["RecommendateProgram"].ToString()))
                .ForMember(dest => dest.File1,
                    opt => opt.MapFrom(src => src["File1"] == DBNull.Value ? null : src["File1"].ToString()))
                .ForMember(dest => dest.File2,
                    opt => opt.MapFrom(src => src["File2"] == DBNull.Value ? null : src["File2"].ToString()))
                .ForMember(dest => dest.File3,
                    opt => opt.MapFrom(src => src["File3"] == DBNull.Value ? null : src["File3"].ToString()))
                .ForMember(dest => dest.File4,
                    opt => opt.MapFrom(src => src["File4"] == DBNull.Value ? null : src["File4"].ToString()))
                .ForMember(dest => dest.File5,
                    opt => opt.MapFrom(src => src["File5"] == DBNull.Value ? null : src["File5"].ToString()))
               .ForMember(dest => dest.entryDateTime,
                    opt => opt.MapFrom(src => src["entryDateTime"] == DBNull.Value ? null : Convert.ToDateTime(src["entryDateTime"]).ToString("dd MMM yyyy")));

            CreateMap<IDataRecord, FeedbackResponse>()
                // For required properties (non-nullable), provide a default (empty string) if DBNull.
                .ForMember(dest => dest.ID,
                    opt => opt.MapFrom(src => src["FeedbackId"] == DBNull.Value ? string.Empty : src["FeedbackId"].ToString()))
                .ForMember(dest => dest.FullName,
                    opt => opt.MapFrom(src => src["fullName"] == DBNull.Value ? string.Empty : src["fullName"].ToString()))
                .ForMember(dest => dest.EmailAddress,
                    opt => opt.MapFrom(src => src["email"] == DBNull.Value ? string.Empty : src["email"].ToString()))
                .ForMember(dest => dest.FeedbackType,
                    opt => opt.MapFrom(src => src["FeedbackType"] == DBNull.Value ? string.Empty : src["FeedbackType"].ToString()))
                // For nullable properties, allow null if DBNull.
                .ForMember(dest => dest.Country,
                    opt => opt.MapFrom(src => src["countryCode"] == DBNull.Value ? null : src["countryCode"].ToString()))
                .ForMember(dest => dest.Subject,
                    opt => opt.MapFrom(src => src["subject"] == DBNull.Value ? null : src["subject"].ToString()))
                .ForMember(dest => dest.Description,
                    opt => opt.MapFrom(src => src["description"] == DBNull.Value ? null : src["description"].ToString()))
                .ForMember(dest => dest.Mobile,
                    opt => opt.MapFrom(src => src["mobile"] == DBNull.Value ? null : src["mobile"].ToString()))                
               .ForMember(dest => dest.TimeStamp,
                    opt => opt.MapFrom(src => src["entryDateTime"] == DBNull.Value ? null : Convert.ToDateTime(src["entryDateTime"]).ToString("dd MMM yyyy")));

            CreateMap<IDataRecord, GrievanceResponse>()
                // For required properties (non-nullable), provide a default (empty string) if DBNull.
                .ForMember(dest => dest.ID,
                    opt => opt.MapFrom(src => src["GrievanceId"] == DBNull.Value ? string.Empty : src["GrievanceId"].ToString()))
                .ForMember(dest => dest.FullName,
                    opt => opt.MapFrom(src => src["fullName"] == DBNull.Value ? string.Empty : src["fullName"].ToString()))
                .ForMember(dest => dest.EmailAddress,
                    opt => opt.MapFrom(src => src["email"] == DBNull.Value ? string.Empty : src["email"].ToString()))
                .ForMember(dest => dest.GrievanceType,
                    opt => opt.MapFrom(src => src["GrievanceType"] == DBNull.Value ? string.Empty : src["GrievanceType"].ToString()))
                // For nullable properties, allow null if DBNull.
                .ForMember(dest => dest.Country,
                    opt => opt.MapFrom(src => src["countryCode"] == DBNull.Value ? null : src["countryCode"].ToString()))
                .ForMember(dest => dest.Subject,
                    opt => opt.MapFrom(src => src["subject"] == DBNull.Value ? null : src["subject"].ToString()))
                .ForMember(dest => dest.Description,
                    opt => opt.MapFrom(src => src["description"] == DBNull.Value ? null : src["description"].ToString()))
                .ForMember(dest => dest.Mobile,
                    opt => opt.MapFrom(src => src["mobile"] == DBNull.Value ? null : src["mobile"].ToString()))
               .ForMember(dest => dest.TimeStamp,
                    opt => opt.MapFrom(src => src["entryDateTime"] == DBNull.Value ? null : Convert.ToDateTime(src["entryDateTime"]).ToString("dd MMM yyyy")));

            CreateMap<IDataRecord, SuggestionResponse>()
                // For required properties (non-nullable), provide a default (empty string) if DBNull.
                .ForMember(dest => dest.ID,
                    opt => opt.MapFrom(src => src["SuggestionId"] == DBNull.Value ? string.Empty : src["SuggestionId"].ToString()))
                .ForMember(dest => dest.FullName,
                    opt => opt.MapFrom(src => src["fullName"] == DBNull.Value ? string.Empty : src["fullName"].ToString()))
                .ForMember(dest => dest.EmailAddress,
                    opt => opt.MapFrom(src => src["email"] == DBNull.Value ? string.Empty : src["email"].ToString()))
                .ForMember(dest => dest.SuggestionType,
                    opt => opt.MapFrom(src => src["SuggestionType"] == DBNull.Value ? string.Empty : src["SuggestionType"].ToString()))
                // For nullable properties, allow null if DBNull.
                .ForMember(dest => dest.Country,
                    opt => opt.MapFrom(src => src["countryCode"] == DBNull.Value ? null : src["countryCode"].ToString()))
                .ForMember(dest => dest.Subject,
                    opt => opt.MapFrom(src => src["subject"] == DBNull.Value ? null : src["subject"].ToString()))
                .ForMember(dest => dest.Description,
                    opt => opt.MapFrom(src => src["description"] == DBNull.Value ? null : src["description"].ToString()))
                .ForMember(dest => dest.Mobile,
                    opt => opt.MapFrom(src => src["mobile"] == DBNull.Value ? null : src["mobile"].ToString()))
               .ForMember(dest => dest.TimeStamp,
                    opt => opt.MapFrom(src => src["entryDateTime"] == DBNull.Value ? null : Convert.ToDateTime(src["entryDateTime"]).ToString("dd MMM yyyy")));

            CreateMap<IDataRecord, BannerResponse>()
               // For required properties (non-nullable), provide a default (empty string) if DBNull.
               .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src["bannerId"]))
               .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src["bannerType"]))
               .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src["bannerName"]))
               .ForMember(dest => dest.Link,
                   opt => opt.MapFrom(src => src["bannerLink"] == DBNull.Value ? string.Empty : src["bannerLink"].ToString()))
               .ForMember(dest => dest.LinkType,
                   opt => opt.MapFrom(src => src["linkType"] == DBNull.Value ? string.Empty : src["linkType"].ToString()))
               .ForMember(dest => dest.ImagePath, opt => opt.MapFrom(src => src["imagePath"]))
               .ForMember(dest => dest.isActive, opt => opt.MapFrom(src => src["isActive"]));

            CreateMap<IDataRecord, EcoSystemResponse>()
               // For required properties (non-nullable), provide a default (empty string) if DBNull.
               .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src["EcoSystemId"]))
               .ForMember(dest => dest.TextOne, opt => opt.MapFrom(src => src["TextOne"]))
               .ForMember(dest => dest.TextTwo, opt => opt.MapFrom(src => src["TextTwo"]))
               .ForMember(dest => dest.ImagePath, opt => opt.MapFrom(src => src["imagePath"]))
               .ForMember(dest => dest.isActive, opt => opt.MapFrom(src => src["isActive"]));

            CreateMap<IDataRecord, PartnersResponse>()
              // For required properties (non-nullable), provide a default (empty string) if DBNull.
              .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src["PartnerId"]))
              .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src["PartnerName"]))
              .ForMember(dest => dest.Link,
                  opt => opt.MapFrom(src => src["PartnerLink"] == DBNull.Value ? string.Empty : src["PartnerLink"].ToString()))
              .ForMember(dest => dest.LinkType,
                  opt => opt.MapFrom(src => src["linkType"] == DBNull.Value ? string.Empty : src["linkType"].ToString()))
              .ForMember(dest => dest.ImagePath, opt => opt.MapFrom(src => src["imagePath"]))
              .ForMember(dest => dest.isActive, opt => opt.MapFrom(src => src["isActive"]));

            CreateMap<IDataRecord, AboutUsResponse>()
            .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src["AboutUsId"]))
            .ForMember(dest => dest.TextOne, opt => opt.MapFrom(src => src["TextOne"]))
            .ForMember(dest => dest.TextTwo, opt => opt.MapFrom(src => src["TextTwo"]))
            .ForMember(dest => dest.ImageFile, opt => opt.MapFrom(src => src["ImagePath"]))
            .ForMember(dest => dest.PDFFile, opt => opt.MapFrom(src => src["PDFPath"]));

            CreateMap<IDataRecord, LeadershipResponse>()
            .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src["LeadershipId"]))
            .ForMember(dest => dest.LeaderName, opt => opt.MapFrom(src => src["LeaderName"]))
            .ForMember(dest => dest.Designation, opt => opt.MapFrom(src => src["Designation"]))
            .ForMember(dest => dest.ImagePath, opt => opt.MapFrom(src => src["ImagePath"]))
            .ForMember(dest => dest.isActive, opt => opt.MapFrom(src => src["IsActive"]));

            CreateMap<IDataRecord, InitiativeResponse>()
                .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src["InitiativeId"]))
                .ForMember(dest => dest.InitiativeName, opt => opt.MapFrom(src => src["InitiativeName"]))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src["InitiativeDesc"]))
                .ForMember(dest => dest.Link,
                  opt => opt.MapFrom(src => src["InitiativeLink"] == DBNull.Value ? string.Empty : src["InitiativeLink"].ToString()))
                .ForMember(dest => dest.LinkType,
                  opt => opt.MapFrom(src => src["linkType"] == DBNull.Value ? string.Empty : src["linkType"].ToString()))
                .ForMember(dest => dest.ImagePath, opt => opt.MapFrom(src => src["ImagePath"]))
                .ForMember(dest => dest.isActive, opt => opt.MapFrom(src => src["IsActive"]));

            CreateMap<IDataRecord, ExecutiveResponse>()
                .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src["ExecutiveId"]))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src["ExecutiveName"]))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src["ExecutiveDesc"]))
                .ForMember(dest => dest.ImagePath, opt => opt.MapFrom(src => src["ImagePath"]))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src["isActive"]));

            CreateMap<IDataRecord, GoverningCouncilResponse>()
                .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src["GoverningCouncilId"]))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src["Name"]))
                .ForMember(dest => dest.Designation, opt => opt.MapFrom(src => src["Designation"]))
                .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src["Position"]))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src["Address"]))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src["isActive"]));

            CreateMap<IDataRecord, GeneralBodyResponse>()
                .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src["GeneralBodyId"]))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src["Name"]))
                .ForMember(dest => dest.Designation, opt => opt.MapFrom(src => src["Designation"]))
                .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src["Position"]))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src["Address"]))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src["isActive"]));

            CreateMap<IDataRecord, VideoResponse>()
                .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src["VideoId"]))
                .ForMember(dest => dest.Link, opt => opt.MapFrom(src => src["Link"]))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src["Title"]))
                .ForMember(dest => dest.IsFeatured, opt => opt.MapFrom(src => src["IsFeatured"]))
                .ForMember(dest => dest.IsLatest, opt => opt.MapFrom(src => src["IsLatest"]))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src["IsActive"]));

            CreateMap<IDataRecord, FeaturedResourceResponse>()
                .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src["FeaturedResourceId"]))
                .ForMember(dest => dest.Link, opt => opt.MapFrom(src => src["Link"]))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src["Title"]))
                .ForMember(dest => dest.FeaturedResourceDate, opt => opt.MapFrom(src => src["FeaturedResourceDate"]))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src["IsActive"]));

            CreateMap<IDataRecord, GovernmentResolutionResponse>()
                .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src["GovernmentResolutionId"]))
                .ForMember(dest => dest.Link, opt => opt.MapFrom(src => src["Link"]))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src["Title"]))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src["IsActive"]));

            CreateMap<IDataRecord, TenderNotificationResponse>()
                .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src["TenderNotificationId"]))
                .ForMember(dest => dest.TenderId, opt => opt.MapFrom(src => src["TenderId"]))
                .ForMember(dest => dest.RefNo, opt => opt.MapFrom(src => src["RefNo"]))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src["Title"]))
                .ForMember(dest => dest.PublishedDate, opt => opt.MapFrom(src => src["PublishedDate"]))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src["Category"]))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src["Status"]))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src["IsActive"]));

            CreateMap<IDataRecord, EventResponse>()
                .ForMember(dest => dest.EventId, opt => opt.MapFrom(src => src["EventId"]))
                .ForMember(dest => dest.EventType, opt => opt.MapFrom(src => src["EventType"]))
                .ForMember(dest => dest.EventDate, opt => opt.MapFrom(src => src["EventDate"]))
                .ForMember(dest => dest.EventLocation, opt => opt.MapFrom(src => src["EventLocation"]))
                .ForMember(dest => dest.ImagePath, opt => opt.MapFrom(src => src["ImagePath"]))
                .ForMember(dest => dest.EventDesc, opt => opt.MapFrom(src => src["EventDesc"]))
                .ForMember(dest => dest.EventName, opt => opt.MapFrom(src => src["EventName"]))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src["IsActive"]));

            CreateMap<IDataRecord, MediaResponse>()
                .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src["MediaId"]))
                .ForMember(dest => dest.Link, opt => opt.MapFrom(src => src["MediaLink"]))
                .ForMember(dest => dest.MediaDate, opt => opt.MapFrom(src => src["MediaDate"]))
                .ForMember(dest => dest.ImagePath, opt => opt.MapFrom(src => src["ImagePath"]))
                .ForMember(dest => dest.MediaDesc, opt => opt.MapFrom(src => src["MediaDesc"]))
                .ForMember(dest => dest.MediaName, opt => opt.MapFrom(src => src["MediaName"]))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src["IsActive"]));

            CreateMap<IDataRecord, LinkedInResponse>()
               .ForMember(dest => dest.ID, opt => opt.MapFrom(src => src["LinkedInId"]))
               .ForMember(dest => dest.Link, opt => opt.MapFrom(src => src["Link"]))
               .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src["Title"]))
               .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src["IsActive"]));
        }
    }
}
