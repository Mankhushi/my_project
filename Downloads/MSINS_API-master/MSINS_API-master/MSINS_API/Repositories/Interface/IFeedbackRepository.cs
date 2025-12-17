using MSINS_API.Models.Request;
using MSINS_API.Models.Response;

namespace MSINS_API.Repositories.Interface
{
    public interface IFeedbackRepository
    {
        Task<int> ExecuteFeedbackProcedureAsync(FeedbackRequestModel request);

        Task<PagedResponse<FeedbackResponse>> GetAllFeedback(int pageIndex, int pageSize, string searchTerm, bool isExport);
    }
}
