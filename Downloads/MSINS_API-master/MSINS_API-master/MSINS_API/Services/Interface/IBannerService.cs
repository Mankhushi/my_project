using MSINS_API.Models.Request;
using MSINS_API.Models.Response;

namespace MSINS_API.Services.Interface
{
    public interface IBannerService
    {
        Task<(int StatusCode, string Message)> AddOrUpdateBannerAsync(BannerRequest bannerDto);

        Task<PagedResponse<BannerResponse>> GetBannersAsync(BannerQueryParamsRequest queryParams);
    }
}
