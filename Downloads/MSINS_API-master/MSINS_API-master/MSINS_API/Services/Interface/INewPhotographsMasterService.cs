using MSINS_API.Models.Request;
using MSINS_API.Models.Response;

namespace MSINS_API.Services.Interface
{
    public interface INewPhotographsMasterService
    {
        // ADD
        Task<(int Code, string Message)> AddPhotographAsync(
            NewPhotographsMasterRequest request);

        // UPDATE
        Task<(int Code, string Message)> UpdatePhotographAsync(
            int photographId,
            NewPhotographsMasterRequest request);

        // GET ALL
        Task<List<NewPhotographsMasterResponse>> GetPhotographsAsync(
            int? initiativeId,
            bool? isActive);

        // GET BY ID
        Task<NewPhotographsMasterResponse?> GetPhotographByIdAsync(int photographId);
    }
}
