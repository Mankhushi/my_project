using MSINS_API.Models.Request;
using MSINS_API.Models.Response;

namespace MSINS_API.Services.Interface
{
    public interface IFeaturedResourceService
    {
        Task<(int Code, string Message)> AddOrUpdateFeaturedResourceAsync(FeaturedResourceRequest featuredDto);

        Task<PagedResponse<FeaturedResourceResponse>> GetFeaturedResourceAsync(FeaturedResourceQueryParamsRequest queryParams);
    }
}
