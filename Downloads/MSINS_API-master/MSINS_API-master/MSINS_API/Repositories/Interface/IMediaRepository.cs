using MSINS_API.Models.Request;
using MSINS_API.Models.Response;

namespace MSINS_API.Repositories.Interface
{
    public interface IMediaRepository
    {
        Task<(int Code, string Message)> AddOrUpdateMediaAsync(MediaRequest MediaDto, string? fileUrl);

        Task<PagedResponse<MediaResponse>> GetMediasAsync(MediaQueryParamRequest queryParams);
    }
}
