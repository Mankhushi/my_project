using MSINS_API.Models.Request;
using MSINS_API.Models.Response;
using MSINS_API.Repositories.Interface;
using MSINS_API.Services.Interface;

namespace MSINS_API.Services.Implementation
{
    public class GeneralBodyService : IGeneralBodyService
    {
        private readonly IFileUploadCustomSizeService _fileUploadService;
        private readonly IGeneralBodyRepository _generalRepository;

        public GeneralBodyService(IFileUploadCustomSizeService fileUploadService, IGeneralBodyRepository generalRepository)
        {
            _fileUploadService = fileUploadService;
            _generalRepository = generalRepository;
        }


        public async Task<(int Code, string Message)> AddOrUpdateGeneralBodyAsync(GeneralBodyRequest generalDto)
        {
            var (resultCode, message) = await _generalRepository.AddOrUpdateGeneralBodyAsync(generalDto);

            if (resultCode.Equals(1))
            {
                return (200, message);
            }
            else
            {
                return (500, "An unexpected error occurred.");
            }
        }

        public async Task<PagedResponse<GeneralBodyResponse>> GetGeneralBodyAsync(GeneralBodyQueryParamRequest queryParams)
        {
            return await _generalRepository.GetGeneralBodyAsync(queryParams);
        }
    }
}
