using MSINS_API.Controllers;
using MSINS_API.Models.Request;
using MSINS_API.Models.Response;

namespace MSINS_API.Repositories.Interface
{
    public interface INewCityMasterRepository
    {
        //Task<(int Code, string Message)> AddCityAsync(NewCityMasterRequest dto);
        Task<(int Code, string Message)> UpdateCityAsync(int cityId, NewCityMasterRequest dto);
        Task<NewCityMasterResponse?> GetCityByIdAsync(int cityId);
        Task<PagedResponse<NewCityMasterResponse>> GetCityListAsync(bool? isActive);
    }
}
