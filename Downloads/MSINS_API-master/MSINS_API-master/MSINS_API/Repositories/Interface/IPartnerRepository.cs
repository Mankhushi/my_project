using MSINS_API.Models.Request;
using MSINS_API.Models.Response;

namespace MSINS_API.Repositories.Interface
{
    public interface IPartnerRepository
    {
        Task<(int Code, string Message)> AddOrUpdatePartnerAsync(PartnersRequest partnerDto, string? fileUrl);

        Task<PagedResponse<PartnersResponse>> GetPartnersAsync(PartnersQueryParamsRequest queryParams);
    }
}
