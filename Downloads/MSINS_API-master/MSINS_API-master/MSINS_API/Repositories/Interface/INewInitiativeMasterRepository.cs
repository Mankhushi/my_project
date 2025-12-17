using MSINS_API.Models.Request;
using MSINS_API.Models.Response;

namespace MSINS_API.Repositories.Interface
{
    public interface INewInitiativeMasterRepository
    {
        /// <summary>
        /// Add a new initiative.
        /// </summary>
        Task<(int Code, string Message)> AddInitiativeMasterAsync(
            NewInitiativeMasterRequest request,
            string? imageUrl);

        /// <summary>
        /// Update an existing initiative.
        /// </summary>
        Task<(int Code, string Message)> UpdateInitiativeMasterAsync(
            int id,
            NewInitiativeMasterRequest request,
            string? imageUrl);

        /// <summary>
        /// Get a single initiative by ID.
        /// </summary>
        Task<NewInitiativeMasterResponse?> GetInitiativeMasterByIdAsync(int id);

        /// <summary>
        /// Get paginated and filtered initiative list.
        /// </summary>
        Task<(List<NewInitiativeMasterResponse> Data, int TotalRecords)>
            GetInitiativeMasterAsync(string? title, bool? isActive, int pageIndex, int pageSize);
    }
}
