using MSINS_API.Models.Request;
using MSINS_API.Models.Response;

namespace MSINS_API.Services.Interface
{
    public interface ILeadershipService
    {
        Task<(int StatusCode, string Message)> AddOrUpdateLeadershipAsync(LeadershipRequest leaderDto);

        Task<PagedResponse<LeadershipResponse>> GetLeadersAsync(LeaderQueryParamsRequest queryParams);
    }
}
