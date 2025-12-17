using MSINS_API.Models.Request;
using MSINS_API.Models.Response;

namespace MSINS_API.Repositories.Interface
{
    public interface IFeaturedResourceRepository
    {
        Task<(int Code, string Message)> AddOrUpdateFeaturedResourceAsync(FeaturedResourceRequest featuredDto, string? fileUrl);

        Task<PagedResponse<FeaturedResourceResponse>> GetFeaturedResourceAsync(FeaturedResourceQueryParamsRequest queryParams);
    }
}
