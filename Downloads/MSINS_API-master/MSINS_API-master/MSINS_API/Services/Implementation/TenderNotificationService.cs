using MSINS_API.Models.Request;
using MSINS_API.Models.Response;
using MSINS_API.Repositories.Interface;
using MSINS_API.Services.Interface;

namespace MSINS_API.Services.Implementation
{
    public class TenderNotificationService : ITenderNotificationService
    {
        private readonly IFileUploadCustomSizeService _fileUploadService;
        private readonly ITenderNotificationRepository _tenderRepository;

        public TenderNotificationService(IFileUploadCustomSizeService fileUploadService, ITenderNotificationRepository tenderRepository)
        {
            _fileUploadService = fileUploadService;
            _tenderRepository = tenderRepository;
        }

        public async Task<(int Code, string Message)> AddOrUpdateTenderNotificationAsync(TenderNotificationRequest tenderDto)
        {
            var (resultCode, message) = await _tenderRepository.AddOrUpdateTenderNotificationAsync(tenderDto);

            if (resultCode.Equals(1))
            {
                return (200, message);
            }
            else
            {
                return (500, "An unexpected error occurred.");
            }
        }

        public async Task<PagedResponse<TenderNotificationResponse>> GetTenderNotificationAsync(TenderNotificationQueryParamsRequest queryParams)
        {
            return await _tenderRepository.GetTenderNotificationAsync(queryParams);
        }
    }
}
