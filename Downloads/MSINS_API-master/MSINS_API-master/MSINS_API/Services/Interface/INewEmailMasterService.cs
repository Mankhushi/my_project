using MSINS_API.Models.Request;
using MSINS_API.Models.Response;
using MSINS_API.Services.Implementation;

namespace MSINS_API.Services.Interface
{
    public interface INewEmailMasterService
    {
        Task<(int Code, string Message, int EmailId)> AddEmailAsync(NewEmailMasterRequest request);
        Task<(int Code, string Message)> UpdateEmailAsync(int id, NewEmailMasterRequest request);
        Task<NewEmailMasterResponse?> GetEmailByIdAsync(int id);

        Task<PagedResponse<NewEmailMasterResponse>> GetAllEmailsAsync(int pageNumber, int pageSize, string? searchTerm, bool? isActive);

        Task<PagedResponse<NewEmailMasterResponse>> GetAllEmailsAsync(NewEmailMasterQueryParams queryParams);

        // 🟢 Add this method
        Task<List<NewEmailMasterResponse>> ExportEmailsAsync(bool? isActive);
    }
}
