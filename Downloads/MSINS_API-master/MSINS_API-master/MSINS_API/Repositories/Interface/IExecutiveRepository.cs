using MSINS_API.Models.Request;
using MSINS_API.Models.Response;

namespace MSINS_API.Repositories.Interface
{
    public interface IExecutiveRepository
    {
        Task<(int Code, string Message)> AddOrUpdateExecutiveAsync(ExecutiveRequest executiveDto, string? fileUrl);

        Task<PagedResponse<ExecutiveResponse>> GetExecutiveAsync(ExecutiveQueryParamsRequest queryParams);
    }
}
