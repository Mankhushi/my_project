using MSINS_API.Models.Request;
using MSINS_API.Models.Response;

namespace MSINS_API.Services.Interface
{
    public interface IVideoService
    {
        Task<(int StatusCode, string Message)> AddOrUpdateVideoAsync(VideoRequest videoDto);

        Task<PagedResponse<VideoResponse>> GetVideosAsync(VideoQueryParamsRequest queryParams);
    }
}
