using MSINS_API.Models.Request;
using MSINS_API.Models.Response;

namespace MSINS_API.Services.Interface
{
    public interface IGeneralBodyService
    {
        Task<(int Code, string Message)> AddOrUpdateGeneralBodyAsync(GeneralBodyRequest generalDto);

        Task<PagedResponse<GeneralBodyResponse>> GetGeneralBodyAsync(GeneralBodyQueryParamRequest queryParams);
    }
}
