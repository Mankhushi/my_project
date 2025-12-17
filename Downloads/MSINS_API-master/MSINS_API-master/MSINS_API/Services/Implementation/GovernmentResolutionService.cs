using MSINS_API.Models.Request;
using MSINS_API.Models.Response;
using MSINS_API.Repositories.Interface;
using MSINS_API.Services.Interface;
using System.Net;

namespace MSINS_API.Services.Implementation
{
    public class GovernmentResolutionService : IGovernmentResolutionService
    {
        private readonly IGovernmentResolutionRepository _featuredRepository;
        private readonly IFileUploadCustomSizeService _fileUploadService;

        private readonly string[] _allowedImageFormats = { "application/pdf" };

        public GovernmentResolutionService(IGovernmentResolutionRepository featuredRepository, IFileUploadCustomSizeService fileUploadService)
        {
            _featuredRepository = featuredRepository;
            _fileUploadService = fileUploadService;
        }

        public async Task<(int Code, string Message)> AddOrUpdateGovernmentResolutionAsync(GovernmentResolutionRequest featuredDto)
        {
            string? fileUrl = null;

            // Handle File Upload (if provided)
            if (featuredDto.PDFFile == null && (featuredDto.GovernmentResolutionId == null || featuredDto.GovernmentResolutionId.Equals(0)))
            {
                return ((int)HttpStatusCode.BadRequest, $"Please select a PDF to upload.");
            }
            else if (featuredDto.PDFFile != null)
            {
                // Validate file format
                if (!_allowedImageFormats.Contains(featuredDto.PDFFile.ContentType.ToLower()))
                {
                    return ((int)HttpStatusCode.BadRequest, "Invalid file type. Only PDF allowed.");
                }


                var uploadResult = await _fileUploadService.UploadFileAsync(featuredDto.PDFFile, "uploads/GovernmentResolution");
                if (!uploadResult.IsSuccess)
                {
                    return ((int)HttpStatusCode.BadRequest, uploadResult.ErrorMessage);
                }
                fileUrl = uploadResult.FileUrl;
            }
            var (resultCode, message) = await _featuredRepository.AddOrUpdateGovernmentResolutionAsync(featuredDto, fileUrl);

            if (resultCode.Equals(1))
            {
                return (200, message);
            }
            else
            {
                return (500, "An unexpected error occurred.");
            }
        }

        public async Task<PagedResponse<GovernmentResolutionResponse>> GetGovernmentResolutionAsync(GovernmentResolutionQueryParamsRequest queryParams)
        {
            return await _featuredRepository.GetGovernmentResolutionAsync(queryParams);
        }
    }
}
