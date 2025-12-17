using MSINS_API.Models.Request;
using MSINS_API.Models.Response;

namespace MSINS_API.Services.Interface
{
    public interface INewSectorMasterService
    {
        // Add a new sector
        Task<(int Code, string Message)> AddSectorAsync(NewSectorMasterRequest sectorRequest);

        // Update existing sector
        Task<(int Code, string Message)> UpdateSectorAsync(NewSectorMasterRequest sectorRequest);

        // Get a single sector by ID
        Task<NewSectorMasterResponse?> GetSectorByIdAsync(int sectorId);

        // ⭐ GET ALL for Website (Only IsActive filter)
        Task<List<NewSectorMasterResponse>> GetSectorsForWebsiteAsync(bool? isActive);
    }
}
