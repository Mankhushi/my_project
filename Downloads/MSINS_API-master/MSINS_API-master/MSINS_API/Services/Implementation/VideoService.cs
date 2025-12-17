using MSINS_API.Models.Request;
using MSINS_API.Models.Response;
using MSINS_API.Repositories.Implementation;
using MSINS_API.Repositories.Interface;
using MSINS_API.Services.Interface;

namespace MSINS_API.Services.Implementation
{
    public class VideoService : IVideoService
    {
        private readonly IVideoRepository _videoRepository;

        public VideoService(IVideoRepository videoRepository)
        {
            _videoRepository = videoRepository;
        }

        public async Task<(int StatusCode, string Message)> AddOrUpdateVideoAsync(VideoRequest videoDto)
        {
            var (resultCode, message) = await _videoRepository.AddOrUpdateVideoAsync(videoDto);

            if (resultCode.Equals(1))
            {
                return (200, message);
            }
            else
            {
                return (500, "An unexpected error occurred.");
            }
        }

        public async Task<PagedResponse<VideoResponse>> GetVideosAsync(VideoQueryParamsRequest queryParams)
        {
            return await _videoRepository.GetVideosAsync(queryParams);
        }
    }
}
