using MSINS_API.Models.Request;
using MSINS_API.Models.Response;

namespace MSINS_API.Services.Interface
{
    public interface INewWinnerMasterService
    {
        // ADD
        Task<(int Code, string Message)> AddWinnerAsync(
            NewWinnerMasterRequest request);

        // UPDATE
        Task<(int Code, string Message)> UpdateWinnerAsync(
            int winnerId,
            NewWinnerMasterRequest request);

        // GET ALL
        Task<List<NewWinnerMasterResponse>> GetWinnersAsync(
            int? sectorId,
            bool? isActive,
            int? initiativeId);

        // GET BY ID
        Task<NewWinnerMasterResponse?> GetWinnerByIdAsync(int winnerId);
    }
}
