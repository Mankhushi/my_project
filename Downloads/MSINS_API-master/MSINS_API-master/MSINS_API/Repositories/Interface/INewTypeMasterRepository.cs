using MSINS_API.Models.Request;
using MSINS_API.Models.Response;

namespace MSINS_API.Repositories.Interface
{
    public interface INewTypeMasterRepository
    {
        Task<(bool IsSuccess, string Message, int TypeId)> AddTypeAsync(NewTypeMasterRequest request);

        Task<(bool IsSuccess, string Message)> UpdateTypeAsync(NewTypeMasterRequest request);

        Task<NewTypeMasterResponse?> GetTypeByIdAsync(int typeId);

        Task<PagedResponse<NewTypeMasterResponse>> GetAllTypesPagedAsync(int pageNumber, int pageSize, string? searchTerm);
    }
}
