using MSINS_API.Models.Request;
using MSINS_API.Models.Response;

namespace MSINS_API.Services.Interface
{
    public interface IFeedbackService
    {
        Task<(int statusCode, string message)> ProcessFeedbackAsync(FeedbackRequestModel request);

        Task<PagedResponse<FeedbackResponse>> GetAllFeedback(int pageIndex, int pageSize, string searchTerm, bool isExport);
    }
}
