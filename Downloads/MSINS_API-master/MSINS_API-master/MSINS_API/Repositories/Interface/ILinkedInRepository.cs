using MSINS_API.Models.Request;
using MSINS_API.Models.Response;

namespace MSINS_API.Repositories.Interface
{
    public interface ILinkedInRepository
    {
        Task<(int Code, string Message)> AddOrUpdateLinkedInAsync(LinkedInRequest LinkedInDto);

        Task<PagedResponse<LinkedInResponse>> GetLinkedInsAsync(LinkedInQueryParamsRequest queryParams);
    }
}
