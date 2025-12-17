using MSINS_API.Models.Request;
using MSINS_API.Models.Response;

namespace MSINS_API.Services.Interface
{
    public interface IInitiativeService
    {
        Task<(int StatusCode, string Message)> AddOrUpdateInitiativeAsync(InitiativeRequest initiativeDto);

        Task<PagedResponse<InitiativeResponse>> GetInitiativeAsync(InitiativeQueryParamsRequest queryParams);
    }
}
