using MSINS_API.Models.Request;
using MSINS_API.Models.Response;
using MSINS_API.Repositories.Interface;
using MSINS_API.Services.Interface;

namespace MSINS_API.Services.Implementation
{
    public class NewTypeMasterService : INewTypeMasterService
    {
        private readonly INewTypeMasterRepository _newTypeMasterRepository;

        public NewTypeMasterService(INewTypeMasterRepository newTypeMasterRepository)
        {
            _newTypeMasterRepository = newTypeMasterRepository;
        }

        // ADD TYPE MASTER
        public async Task<(bool IsSuccess, string Message, int TypeId)> AddTypeAsync(NewTypeMasterRequest request)
        {
            return await _newTypeMasterRepository.AddTypeAsync(request);
        }

        // UPDATE TYPE MASTER
        public async Task<(bool IsSuccess, string Message)> UpdateTypeAsync(NewTypeMasterRequest request)
        {
            return await _newTypeMasterRepository.UpdateTypeAsync(request);
        }

        // GET TYPE MASTER BY ID
        public async Task<NewTypeMasterResponse?> GetTypeByIdAsync(int typeId)
        {
            return await _newTypeMasterRepository.GetTypeByIdAsync(typeId);
        }

        // ⭐ GET ALL TYPES (PAGINATION + SEARCH + SECTOR NAME)
        public async Task<PagedResponse<NewTypeMasterResponse>> GetAllTypesAsync(
            int pageNumber,
            int pageSize,
            string? searchTerm)
        {
            return await _newTypeMasterRepository
                .GetAllTypesPagedAsync(pageNumber, pageSize, searchTerm);
        }
    }
}
