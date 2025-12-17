using MSINS_API.Models.Request;
using MSINS_API.Models.Response;

namespace MSINS_API.Repositories.Interface
{
    public interface IBannerRepository
    {
        Task<(int Code, string Message)> AddOrUpdateBannerAsync(BannerRequest bannerDto, string? fileUrl);

        Task<PagedResponse<BannerResponse>> GetBannersAsync(BannerQueryParamsRequest queryParams);
    }
}
