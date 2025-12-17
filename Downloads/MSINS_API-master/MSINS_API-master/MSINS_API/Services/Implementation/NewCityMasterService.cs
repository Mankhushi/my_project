using MSINS_API.Controllers;
using MSINS_API.Models.Request;
using MSINS_API.Models.Response;
using MSINS_API.Repositories.Interface;
using MSINS_API.Services.Interface;
using System.Net;

namespace MSINS_API.Services.Implementation
{
    public class NewCityMasterService : INewCityMasterService
    {
        private readonly INewCityMasterRepository _cityRepository;

        public NewCityMasterService(INewCityMasterRepository cityRepository)
        {
            _cityRepository = cityRepository;
        }
        //------------------------------- ADD CITY---------------------------------------
        
        //---------------------------------UPDATE CITY----------------------------------------------
        public async Task<(int Code, string Message)> UpdateCityAsync(int cityId, NewCityMasterRequest dto)
        {
            if (cityId <= 0)
                return ((int)HttpStatusCode.BadRequest, "Invalid City ID.");

            if (string.IsNullOrWhiteSpace(dto.CityName))
                return ((int)HttpStatusCode.BadRequest, "City name is required.");

            var (resultCode, message) = await _cityRepository.UpdateCityAsync(cityId, dto);

            if (resultCode.Equals(1))
            {
                return (200, message);
            }

            return (500, "An unexpected error occurred while updating city.");
        }

        //------------------------------------------------- GET CITY BY ID ---------------------------------------------
        public async Task<NewCityMasterResponse?> GetCityByIdAsync(int cityId)
        {
            if (cityId <= 0)
                return null;

            return await _cityRepository.GetCityByIdAsync(cityId);
        }

        //-------------------------------- GET CITY LIST ---------------------------------------------
        public async Task<PagedResponse<NewCityMasterResponse>> GetCityListAsync(
   
    bool? isActive)
        {
            return await _cityRepository.GetCityListAsync(isActive);
        }
    }
}
