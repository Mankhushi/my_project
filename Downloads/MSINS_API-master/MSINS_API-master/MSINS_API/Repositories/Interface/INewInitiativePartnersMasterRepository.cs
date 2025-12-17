using MSINS_API.Models.Request;
using MSINS_API.Models.Response;

namespace MSINS_API.Repositories.Interface
{
    public interface INewInitiativePartnersMasterRepository
    {
        // ADD
        Task<(int Code, string Message)> AddPartnerAsync(
            NewInitiativePartnersMasterRequest request,
            string? fileUrl);

        // UPDATE
        Task<(int Code, string Message)> UpdatePartnerAsync(
            int partnerId,
            NewInitiativePartnersMasterRequest request,
            string? fileUrl);

        // GET ALL
        Task<List<NewInitiativePartnersMasterResponse>> GetPartnersAsync(
            int? initiativeId,
            bool? isActive);

        // GET BY ID
        Task<NewInitiativePartnersMasterResponse?> GetPartnerByIdAsync(int partnerId);
    }
}
