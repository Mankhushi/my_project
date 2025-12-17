using MSINS_API.Models.Request;
using MSINS_API.Models.Response;
using MSINS_API.Repositories.Implementation;
using MSINS_API.Repositories.Interface;
using MSINS_API.Services.Interface;

namespace MSINS_API.Services.Implementation
{
    public class LinkedInService : ILinkedInService
    {
        private readonly ILinkedInRepository _LinkedInRepository;

        public LinkedInService(ILinkedInRepository LinkedInRepository)
        {
            _LinkedInRepository = LinkedInRepository;
        }

        public async Task<(int Code, string Message)> AddOrUpdateLinkedInAsync(LinkedInRequest LinkedInDto)
        {
            var (resultCode, message) = await _LinkedInRepository.AddOrUpdateLinkedInAsync(LinkedInDto);

            if (resultCode.Equals(1))
            {
                return (200, message);
            }
            else
            {
                return (500, "An unexpected error occurred.");
            }
        }

        public async Task<PagedResponse<LinkedInResponse>> GetLinkedInsAsync(LinkedInQueryParamsRequest queryParams)
        {
            return await _LinkedInRepository.GetLinkedInsAsync(queryParams);
        }
    }
}
