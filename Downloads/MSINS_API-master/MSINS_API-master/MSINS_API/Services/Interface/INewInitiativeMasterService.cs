using MSINS_API.Models.Request;
using MSINS_API.Models.Response;

namespace MSINS_API.Services.Interface
{
    public interface INewInitiativeMasterService
    {
        /// <summary>
        /// Add a new initiative.
        /// </summary>
        Task<(int Code, string Message)> AddInitiativeMasterAsync(NewInitiativeMasterRequest request);

        /// <summary>
        /// Update an existing initiative by ID.
        /// </summary>
        Task<(int Code, string Message)> UpdateInitiativeMasterAsync(int id, NewInitiativeMasterRequest request);

        /// <summary>
        /// Get initiative details by ID.
        /// </summary>
        Task<NewInitiativeMasterResponse?> GetInitiativeMasterByIdAsync(int id);

        /// <summary>
        /// Get a filtered and paginated list of initiatives.
        /// </summary>
        Task<(List<NewInitiativeMasterResponse> Data, int TotalRecords)>
            GetInitiativeMasterAsync(string? title, bool? isActive, int pageIndex, int pageSize);
    }
}
