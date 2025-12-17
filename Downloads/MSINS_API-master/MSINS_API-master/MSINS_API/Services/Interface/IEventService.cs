using MSINS_API.Models.Request;
using MSINS_API.Models.Response;

namespace MSINS_API.Services.Interface
{
    public interface IEventService
    {
        Task<(int StatusCode, string Message)> AddOrUpdateEventAsync(EventRequest EventDto);

        Task<PagedResponse<EventResponse>> GetEventsAsync(EventQueryParamRequest queryParams);
    }
}
