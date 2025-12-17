using MSINS_API.Models.Request;
using MSINS_API.Models.Response;
using MSINS_API.Services.Implementation;

namespace MSINS_API.Repositories.Interface
{
    public interface INewEmailMasterRepository
    {
        Task<(int Code, string Message, int EmailId)> AddEmailAsync(NewEmailMasterRequest request);

        Task<(int Code, string Message)> UpdateEmailAsync(int id, NewEmailMasterRequest request);

        // GET ALL (Direct Params)
        Task<PagedResponse<NewEmailMasterResponse>> GetAllEmailsAsync(
            int pageNumber,
            int pageSize,
            string? searchTerm,
            bool? isActive);

        // GET ALL (QueryParams Wrapper)
        Task<PagedResponse<NewEmailMasterResponse>> GetAllEmailsAsync(
            NewEmailMasterQueryParams queryParams);

        Task<NewEmailMasterResponse?> GetEmailByIdAsync(int id);

        Task<List<NewEmailMasterResponse>> ExportEmailsAsync(bool? isActive);
    }
}
