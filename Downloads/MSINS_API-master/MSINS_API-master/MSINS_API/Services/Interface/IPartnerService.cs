using MSINS_API.Models.Request;
using MSINS_API.Models.Response;

namespace MSINS_API.Services.Interface
{
    public interface IPartnerService
    {
        Task<(int StatusCode, string Message)> AddOrUpdatePartnerAsync(PartnersRequest partnerDto);

        Task<PagedResponse<PartnersResponse>> GetPartnersAsync(PartnersQueryParamsRequest queryParams);
    }
}
