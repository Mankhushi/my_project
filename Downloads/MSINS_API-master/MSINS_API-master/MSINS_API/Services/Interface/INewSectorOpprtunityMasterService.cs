using MSINS_API.Models.Request;
using MSINS_API.Models.Response;

namespace MSINS_API.Services.Interface
{
    public interface INewSectorOpprtunityMasterService
    {
        // ADD
        Task<(int Code, string Message)> AddSectorOpprtunityAsync(
            NewSectorOpprtunityMasterRequest request);

        // UPDATE
        Task<(int Code, string Message)> UpdateSectorOpprtunityAsync(
            int sectorOpprtunityId,
            NewSectorOpprtunityMasterRequest request);

        // GET ALL
        Task<List<NewSectorOpprtunityMasterResponse>> GetSectorOpprtunityAsync(
            bool? isActive,
            int? initiativeId);

        // GET BY ID
        Task<NewSectorOpprtunityMasterResponse?> GetSectorOpprtunityByIdAsync(
            int sectorOpprtunityId);
    }
}
