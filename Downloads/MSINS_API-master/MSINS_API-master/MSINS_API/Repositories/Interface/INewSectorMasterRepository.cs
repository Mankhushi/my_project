using MSINS_API.Models.Request;
using MSINS_API.Models.Response;

namespace MSINS_API.Repositories.Interface
{
    public interface INewSectorMasterRepository
    {
        
        Task<(int Code, string Message)> AddSectorAsync(NewSectorMasterRequest sectorRequest);
        Task<(int Code, string Message)> UpdateSectorAsync(NewSectorMasterRequest sectorRequest);
        Task<NewSectorMasterResponse?> GetSectorByIdAsync(int sectorId);
        Task<List<NewSectorMasterResponse>> GetSectorsForWebsiteAsync(bool? isActive);
    }
}
