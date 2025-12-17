using MSINS_API.Models.Request;
using MSINS_API.Models.Response;

namespace MSINS_API.Repositories.Interface
{
    public interface ITenderNotificationRepository
    {
        Task<(int Code, string Message)> AddOrUpdateTenderNotificationAsync(TenderNotificationRequest generalDto);

        Task<PagedResponse<TenderNotificationResponse>> GetTenderNotificationAsync(TenderNotificationQueryParamsRequest queryParams);
    }
}
