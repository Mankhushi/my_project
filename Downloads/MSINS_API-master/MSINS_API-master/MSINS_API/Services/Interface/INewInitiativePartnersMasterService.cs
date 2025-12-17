using MSINS_API.Models.Request;
using MSINS_API.Models.Response;

namespace MSINS_API.Services.Interface
{
    public interface INewInitiativePartnersMasterService
    {
        // ADD PARTNER
        Task<(int Code, string Message)> AddPartnerAsync(
            NewInitiativePartnersMasterRequest request,
            string? fileUrl);

        // UPDATE PARTNER
        Task<(int Code, string Message)> UpdatePartnerAsync(
            int partnerId,
            NewInitiativePartnersMasterRequest request,
            string? fileUrl);

        // GET ALL PARTNERS
        Task<List<NewInitiativePartnersMasterResponse>> GetPartnersAsync(
            int? initiativeId,
            bool? isActive);

        // GET PARTNER BY ID
        Task<NewInitiativePartnersMasterResponse?> GetPartnerByIdAsync(int partnerId);
    }
}
