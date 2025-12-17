using MSINS_API.Models.Request;
using MSINS_API.Models.Response;

namespace MSINS_API.Services.Interface
{
    public interface INewTypeMasterService
    {
        Task<(bool IsSuccess, string Message, int TypeId)> AddTypeAsync(NewTypeMasterRequest request);

        Task<(bool IsSuccess, string Message)> UpdateTypeAsync(NewTypeMasterRequest request);

        Task<NewTypeMasterResponse?> GetTypeByIdAsync(int typeId);

        // ⭐ FINAL METHOD (Pagination + Search)
        Task<PagedResponse<NewTypeMasterResponse>> GetAllTypesAsync(
            int pageNumber,
            int pageSize,
            string? searchTerm
        );
       
    }
}
