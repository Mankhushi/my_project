using MSINS_API.Models.Request;
using MSINS_API.Models.Response;

namespace MSINS_API.Repositories.Interface
{
    public interface ISuggestionRepository
    {
        Task<int> ExecuteSuggestionProcedureAsync(SuggestionRequestModel request);

        Task<PagedResponse<SuggestionResponse>> GetAllSuggestion(int pageIndex, int pageSize, string searchTerm, bool isExport);
    }
}
