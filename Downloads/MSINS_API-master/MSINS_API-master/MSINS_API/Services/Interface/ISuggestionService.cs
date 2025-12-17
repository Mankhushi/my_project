using MSINS_API.Models.Request;
using MSINS_API.Models.Response;

namespace MSINS_API.Services.Interface
{
    public interface ISuggestionService
    {
        Task<(int statusCode, string message)> ProcessSuggestionAsync(SuggestionRequestModel request);

        Task<PagedResponse<SuggestionResponse>> GetAllSuggestion(int pageIndex, int pageSize, string searchTerm, bool isExport);
    }
}
