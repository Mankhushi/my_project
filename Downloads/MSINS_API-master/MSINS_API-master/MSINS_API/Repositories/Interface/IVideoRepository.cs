using MSINS_API.Models.Request;
using MSINS_API.Models.Response;

namespace MSINS_API.Repositories.Interface
{
    public interface IVideoRepository
    {
        Task<(int Code, string Message)> AddOrUpdateVideoAsync(VideoRequest videoDto);

        Task<PagedResponse<VideoResponse>> GetVideosAsync(VideoQueryParamsRequest queryParams);
    }
}
