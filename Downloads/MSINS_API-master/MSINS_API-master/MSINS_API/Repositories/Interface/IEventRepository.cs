using MSINS_API.Models.Request;
using MSINS_API.Models.Response;

namespace MSINS_API.Repositories.Interface
{
    public interface IEventRepository
    {
        Task<(int Code, string Message)> AddOrUpdateEventAsync(EventRequest EventDto, string? fileUrl);

        Task<PagedResponse<EventResponse>> GetEventsAsync(EventQueryParamRequest queryParams);
    }
}
