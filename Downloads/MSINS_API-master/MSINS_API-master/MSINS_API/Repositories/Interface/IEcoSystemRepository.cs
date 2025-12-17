using MSINS_API.Models.Request;
using MSINS_API.Models.Response;

namespace MSINS_API.Repositories.Interface
{
    public interface IEcoSystemRepository
    {
        Task<(int Code, string Message)> AddOrUpdateEcoSystemAsync(EcoSystemRequest ecoSystemDto, string? fileUrl);

        Task<PagedResponse<EcoSystemResponse>> GetEcoSystemAsync(EcoSystemQueryParamsRequest queryParams);
    }
}
