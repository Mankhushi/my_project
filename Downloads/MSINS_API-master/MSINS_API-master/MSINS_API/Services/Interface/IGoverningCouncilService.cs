using MSINS_API.Models.Request;
using MSINS_API.Models.Response;

namespace MSINS_API.Services.Interface
{
    public interface IGoverningCouncilService
    {
        Task<(int Code, string Message)> AddOrUpdateCouncilAsync(GoverningCouncilRequest councilDto);

        Task<PagedResponse<GoverningCouncilResponse>> GetCouncilAsync(GoverningCouncilQueryParamRequest queryParams);
    }
}
