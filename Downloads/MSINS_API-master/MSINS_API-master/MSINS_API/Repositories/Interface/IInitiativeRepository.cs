using MSINS_API.Models.Request;
using MSINS_API.Models.Response;

namespace MSINS_API.Repositories.Interface
{
    public interface IInitiativeRepository
    {
        Task<(int Code, string Message)> AddOrUpdateInitiativeAsync(InitiativeRequest initiativeDto, string? fileUrl);

        Task<PagedResponse<InitiativeResponse>> GetInitiativeAsync(InitiativeQueryParamsRequest queryParams);
    }
}
