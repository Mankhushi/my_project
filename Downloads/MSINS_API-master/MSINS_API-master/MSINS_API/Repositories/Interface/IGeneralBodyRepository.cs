using MSINS_API.Models.Request;
using MSINS_API.Models.Response;

namespace MSINS_API.Repositories.Interface
{
    public interface IGeneralBodyRepository
    {
        Task<(int Code, string Message)> AddOrUpdateGeneralBodyAsync(GeneralBodyRequest generalDto);

        Task<PagedResponse<GeneralBodyResponse>> GetGeneralBodyAsync(GeneralBodyQueryParamRequest queryParams);
    }
}
