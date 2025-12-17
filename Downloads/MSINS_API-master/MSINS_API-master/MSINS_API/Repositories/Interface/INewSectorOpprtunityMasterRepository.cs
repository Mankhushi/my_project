using MSINS_API.Models.Request;
using MSINS_API.Models.Response;

namespace MSINS_API.Repositories.Interface
{
    public interface INewSectorOpprtunityMasterRepository
    {
        // ADD
        Task<(int Code, string Message)> AddSectorOpprtunityAsync(
            NewSectorOpprtunityMasterRequest request,
            string fileUrl);

        // UPDATE
        Task<(int Code, string Message)> UpdateSectorOpprtunityAsync(
            int sectorOpprtunityId,
            NewSectorOpprtunityMasterRequest request,
            string? fileUrl);

        // GET ALL
        Task<List<NewSectorOpprtunityMasterResponse>> GetSectorOpprtunityAsync(
            bool? isActive,
            int? initiativeId);

        // GET BY ID
        Task<NewSectorOpprtunityMasterResponse?> GetSectorOpprtunityByIdAsync(
            int sectorOpprtunityId);
    }
}
