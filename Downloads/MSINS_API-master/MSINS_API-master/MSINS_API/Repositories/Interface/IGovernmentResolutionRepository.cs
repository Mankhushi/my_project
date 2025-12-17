using MSINS_API.Models.Request;
using MSINS_API.Models.Response;

namespace MSINS_API.Repositories.Interface
{
    public interface IGovernmentResolutionRepository
    {
        Task<(int Code, string Message)> AddOrUpdateGovernmentResolutionAsync(GovernmentResolutionRequest featuredDto, string? fileUrl);

        Task<PagedResponse<GovernmentResolutionResponse>> GetGovernmentResolutionAsync(GovernmentResolutionQueryParamsRequest queryParams);
    }
}
