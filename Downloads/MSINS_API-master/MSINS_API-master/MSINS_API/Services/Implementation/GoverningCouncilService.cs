using MSINS_API.Models.Request;
using MSINS_API.Models.Response;
using MSINS_API.Repositories.Implementation;
using MSINS_API.Repositories.Interface;
using MSINS_API.Services.Interface;
using System.Net;

namespace MSINS_API.Services.Implementation
{
    public class GoverningCouncilService : IGoverningCouncilService
    {
        private readonly IFileUploadCustomSizeService _fileUploadService;
        private readonly IGoverningCouncilRepository _councilRepository;

        public GoverningCouncilService(IFileUploadCustomSizeService fileUploadService, IGoverningCouncilRepository councilRepository)
        {
            _fileUploadService = fileUploadService;
            _councilRepository = councilRepository;
        }

        public async Task<(int Code, string Message)> AddOrUpdateCouncilAsync(GoverningCouncilRequest councilDto)
        {
            var (resultCode, message) = await _councilRepository.AddOrUpdateCouncilAsync(councilDto);

            if (resultCode.Equals(1))
            {
                return (200, message);
            }
            else
            {
                return (500, "An unexpected error occurred.");
            }
        }

        public async Task<PagedResponse<GoverningCouncilResponse>> GetCouncilAsync(GoverningCouncilQueryParamRequest queryParams)
        {
            return await _councilRepository.GetCouncilAsync(queryParams);
        }
    }
}
