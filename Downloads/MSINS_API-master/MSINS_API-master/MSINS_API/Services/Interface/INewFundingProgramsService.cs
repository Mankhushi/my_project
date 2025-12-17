using MSINS_API.Models.Request;
using MSINS_API.Models.Response;
using MSINS_API.POCO;

namespace MSINS_API.Services.Interface
{
    public interface INewFundingProgramsService
    {
        Task<(int Code, string Message)> AddFundingProgramAsync(NewFundingProgramsRequest request);

        Task<(int Code, string Message)> UpdateFundingProgramAsync(int fundProgramId, NewFundingProgramsRequest request);

        Task<PagedResponse<NewFundingProgramsResponse>> GetFundingProgramsAsync(string? fundingAgencyName, bool? isActive, int pageIndex, int pageSize);

        Task<NewFundingProgramsResponse?> GetFundingProgramByIdAsync(int fundProgramId);
    }
}
