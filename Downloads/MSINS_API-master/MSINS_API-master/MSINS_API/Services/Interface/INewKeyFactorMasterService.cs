using MSINS_API.Models.Request;
using MSINS_API.Models.Response;

namespace MSINS_API.Services.Interface
{
    public interface INewKeyFactorMasterService
    {
        // ADD
        Task<(int Code, string Message)> AddKeyFactorAsync(NewKeyFactorMasterRequest request);

        // UPDATE
        Task<(int Code, string Message)> UpdateKeyFactorAsync(
            int keyFactorId,
            NewKeyFactorMasterRequest request);

        // GET ALL
        Task<List<NewKeyFactorMasterResponse>> GetKeyFactorsAsync(
            bool? isActive,
            int? initiativeId);

        // GET BY ID
        Task<NewKeyFactorMasterResponse?> GetKeyFactorByIdAsync(int keyFactorId);
    }
}
