using MSINS_API.Models.Request;
using MSINS_API.Models.Response;

namespace MSINS_API.Services.Interface
{
    public interface ILinkedInService
    {
        Task<(int Code, string Message)> AddOrUpdateLinkedInAsync(LinkedInRequest LinkedInDto);

        Task<PagedResponse<LinkedInResponse>> GetLinkedInsAsync(LinkedInQueryParamsRequest queryParams);
    }
}
