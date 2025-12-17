using MSINS_API.Models.Request;
using MSINS_API.Models.Response;

namespace MSINS_API.Repositories.Interface
{
    public interface IGoverningCouncilRepository
    {
        Task<(int Code, string Message)> AddOrUpdateCouncilAsync(GoverningCouncilRequest councilDto);

        Task<PagedResponse<GoverningCouncilResponse>> GetCouncilAsync(GoverningCouncilQueryParamRequest queryParams);
    }
}
