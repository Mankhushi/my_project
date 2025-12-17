using MSINS_API.Models.Request;
using MSINS_API.Models.Response;

namespace MSINS_API.Services.Interface
{
    public interface IMediaService
    {
        Task<(int StatusCode, string Message)> AddOrUpdateMediaAsync(MediaRequest MediaDto);

        Task<PagedResponse<MediaResponse>> GetMediasAsync(MediaQueryParamRequest queryParams);
    }
}
