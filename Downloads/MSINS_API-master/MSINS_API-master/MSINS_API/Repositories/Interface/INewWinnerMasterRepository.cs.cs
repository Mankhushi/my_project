using MSINS_API.Models.Request;
using MSINS_API.Models.Response;

namespace MSINS_API.Repositories.Interface
{
    public interface INewWinnerMasterRepository
    {
        // ADD
        Task<(int Code, string Message)> AddWinnerAsync(
            NewWinnerMasterRequest request,
            string? fileUrl);

        // UPDATE
        Task<(int Code, string Message)> UpdateWinnerAsync(
            int winnerId,
            NewWinnerMasterRequest request,
            string? fileUrl);

        // GET ALL
        Task<List<NewWinnerMasterResponse>> GetWinnersAsync(
            int? sectorId,
            bool? isActive,
            int? initiativeId);

        // GET BY ID
        Task<NewWinnerMasterResponse?> GetWinnerByIdAsync(int winnerId);
    }
}
