using MSINS_API.Models.Request;
using MSINS_API.Models.Response;

namespace MSINS_API.Services.Interface
{
    public interface IEcoSystemService
    {
        Task<(int Code, string Message)> AddOrUpdateEcoSystemAsync(EcoSystemRequest ecoSystemDto);

        Task<PagedResponse<EcoSystemResponse>> GetEcoSystemAsync(EcoSystemQueryParamsRequest queryParams);
    }
}
