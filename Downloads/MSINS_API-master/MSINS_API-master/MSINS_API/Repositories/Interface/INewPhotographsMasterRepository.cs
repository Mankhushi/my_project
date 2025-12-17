using MSINS_API.Models.Request;
using MSINS_API.Models.Response;

namespace MSINS_API.Repositories.Interface
{
    public interface INewPhotographsMasterRepository
    {
        // ADD
        Task<(int Code, string Message)> AddPhotographAsync(
            NewPhotographsMasterRequest request,
            string? fileUrl);

        // UPDATE
        Task<(int Code, string Message)> UpdatePhotographAsync(
            int photographId,
            NewPhotographsMasterRequest request,
            string? fileUrl);

        // GET ALL
        Task<List<NewPhotographsMasterResponse>> GetPhotographsAsync(
            int? initiativeId,
            bool? isActive);

        // GET BY ID
        Task<NewPhotographsMasterResponse?> GetPhotographByIdAsync(
            int photographId);
    }
}
