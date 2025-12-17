using MSINS_API.Models.Request;
using MSINS_API.Models.Response;
using MSINS_API.Repositories.Interface;
using MSINS_API.Services.Interface;
using System.Net;

namespace MSINS_API.Services.Implementation
{
    public class NewSectorMasterService : INewSectorMasterService
    {
        private readonly INewSectorMasterRepository _sectorRepository;

        public NewSectorMasterService(INewSectorMasterRepository sectorRepository)
        {
            _sectorRepository = sectorRepository;
        }

        // ⭐ ADD SECTOR
        public async Task<(int Code, string Message)> AddSectorAsync(NewSectorMasterRequest sectorRequest)
        {
            if (string.IsNullOrWhiteSpace(sectorRequest.SectorName))
                return ((int)HttpStatusCode.BadRequest, "Sector name is required.");

            var (resultCode, message) = await _sectorRepository.AddSectorAsync(sectorRequest);

            return resultCode switch
            {
                200 => ((int)HttpStatusCode.OK, message),
                400 => ((int)HttpStatusCode.BadRequest, message),
                _ => ((int)HttpStatusCode.InternalServerError, "An unexpected error occurred.")
            };
        }


        // ⭐ UPDATE SECTOR
        public async Task<(int Code, string Message)> UpdateSectorAsync(NewSectorMasterRequest sectorRequest)
        {
            if (string.IsNullOrWhiteSpace(sectorRequest.SectorName))
                return ((int)HttpStatusCode.BadRequest, "Sector name is required.");

            var (resultCode, message) = await _sectorRepository.UpdateSectorAsync(sectorRequest);

            return resultCode switch
            {
                200 => ((int)HttpStatusCode.OK, message),
                400 => ((int)HttpStatusCode.BadRequest, message),
                404 => ((int)HttpStatusCode.NotFound, message),
                _ => ((int)HttpStatusCode.InternalServerError, "An unexpected error occurred.")
            };
        }


        // ⭐ GET BY ID
        public async Task<NewSectorMasterResponse?> GetSectorByIdAsync(int sectorId)
        {
            if (sectorId <= 0)
                throw new ArgumentException("Invalid SectorId.");

            return await _sectorRepository.GetSectorByIdAsync(sectorId);
        }


        // ⭐ GET ALL (Pagination + Search)
        public async Task<List<NewSectorMasterResponse>> GetSectorsForWebsiteAsync(bool? isActive)
        {
            var result = await _sectorRepository.GetSectorsForWebsiteAsync(isActive);
            return result;
        }

    }
}
