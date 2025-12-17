using MSINS_API.Models.Request;
using MSINS_API.Models.Response;

namespace MSINS_API.Services.Interface
{
    public interface IExecutiveService
    {
        Task<(int StatusCode, string Message)> AddOrUpdateExecutiveAsync(ExecutiveRequest executiveDto);

        Task<PagedResponse<ExecutiveResponse>> GetExecutiveAsync(ExecutiveQueryParamsRequest queryParams);
    }
}
