using MSINS_API.Controllers;
using MSINS_API.Models.Request;
using MSINS_API.Models.Response;


namespace MSINS_API.Services.Interface
{
    public interface INewCityMasterService
    {
        //Task<(int Code, string Message)> AddCityAsync(NewCityMasterRequest dto);
        Task<(int Code, string Message)> UpdateCityAsync(int cityId, NewCityMasterRequest dto);
        Task<NewCityMasterResponse?> GetCityByIdAsync(int cityId);
        Task<PagedResponse<NewCityMasterResponse>> GetCityListAsync(bool? isActive);
    }
}
