using MSINS_API.Models.Request;
using MSINS_API.Models.Response;

namespace MSINS_API.Repositories.Interface
{
    public interface ILeadershipRepository
    {
        Task<(int Code, string Message)> AddOrUpdateLeadershipAsync(LeadershipRequest leaderDto, string? fileUrl);

        Task<PagedResponse<LeadershipResponse>> GetLeadersAsync(LeaderQueryParamsRequest queryParams);
    }
}
