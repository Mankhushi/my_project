using MSINS_API.Models.Request;
using MSINS_API.Models.Response;

namespace MSINS_API.Repositories.Interface
{
    public interface INewKeyFactorMasterRepository
    {
        // ADD
        Task<(int Code, string Message)> AddKeyFactorAsync(
            NewKeyFactorMasterRequest request,
            string? fileUrl);

        // UPDATE
        Task<(int Code, string Message)> UpdateKeyFactorAsync(
            int keyFactorId,
            NewKeyFactorMasterRequest request,
            string? fileUrl);

        // GET ALL
        Task<List<NewKeyFactorMasterResponse>> GetKeyFactorsAsync(
            bool? isActive,
            int? initiativeId);

        // GET BY ID
        Task<NewKeyFactorMasterResponse?> GetKeyFactorByIdAsync(int keyFactorId);
    }
}
