using MSINS_API.Models.Request;
using MSINS_API.Models.Response;

namespace MSINS_API.Services.Interface
{
    public interface IGovernmentResolutionService
    {
        Task<(int Code, string Message)> AddOrUpdateGovernmentResolutionAsync(GovernmentResolutionRequest featuredDto);

        Task<PagedResponse<GovernmentResolutionResponse>> GetGovernmentResolutionAsync(GovernmentResolutionQueryParamsRequest queryParams);
    }
}
