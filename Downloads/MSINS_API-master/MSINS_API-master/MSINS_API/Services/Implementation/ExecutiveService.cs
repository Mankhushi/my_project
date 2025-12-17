using MSINS_API.Models.Request;
using MSINS_API.Models.Response;
using MSINS_API.Repositories.Implementation;
using MSINS_API.Repositories.Interface;
using MSINS_API.Services.Interface;
using SixLabors.ImageSharp;
using System.Net;

namespace MSINS_API.Services.Implementation
{
    public class ExecutiveService : IExecutiveService
    {
        private readonly IFileUploadCustomSizeService _fileUploadService;
        private readonly IExecutiveRepository _executiveRepository;
        private const long _maxImageSizeInKb = 500;  // 50 KB
        private const int _requiredWidth = 150;
        private const int _requiredHeight = 150;
        private readonly string[] _allowedImageFormats = { "image/jpg", "image/jpeg", "image/png", "image/webp" };

        public ExecutiveService(IFileUploadCustomSizeService fileUploadService, IExecutiveRepository executiveRepository)
        {
            _fileUploadService = fileUploadService;
            _executiveRepository = executiveRepository;
        }

        public async Task<(int StatusCode, string Message)> AddOrUpdateExecutiveAsync(ExecutiveRequest executiveDto)
        {
            string? fileUrl = null;
            if (executiveDto.ImageFile == null && (executiveDto.ExecutiveId == null || executiveDto.ExecutiveId.Equals(0)))
            {
                return ((int)HttpStatusCode.BadRequest, $"Please select an image to upload.");
            }
            else if (executiveDto.ImageFile != null)
            {
                using (var stream = executiveDto.ImageFile.OpenReadStream())
                using (var image = Image.Load(stream))
                {
                    if (image.Width != _requiredWidth || image.Height != _requiredHeight)
                    {
                        return ((int)HttpStatusCode.BadRequest, $"Image must be exactly {_requiredWidth}x{_requiredHeight}px.");
                    }

                    // Validate file format
                    if (!_allowedImageFormats.Contains(executiveDto.ImageFile.ContentType.ToLower()))
                    {
                        return ((int)HttpStatusCode.BadRequest, "Invalid file type. Only JPG and PNG are allowed.");
                    }

                    // Validate file size
                    if (executiveDto.ImageFile.Length > _maxImageSizeInKb * 1024) // Convert KB to Bytes
                    {
                        return ((int)HttpStatusCode.BadRequest, $"File size exceeds {_maxImageSizeInKb} KB limit.");
                    }
                }

                var uploadResult = await _fileUploadService.UploadFileAsync(executiveDto.ImageFile, "uploads/executives");
                if (!uploadResult.IsSuccess)
                {
                    return ((int)HttpStatusCode.BadRequest, uploadResult.ErrorMessage);
                }
                fileUrl = uploadResult.FileUrl;
            }

            var (resultCode, message) = await _executiveRepository.AddOrUpdateExecutiveAsync(executiveDto, fileUrl);

            if (resultCode.Equals(1))
            {
                return (200, message);
            }
            else
            {
                return (500, "An unexpected error occurred.");
            }
        }

        public async Task<PagedResponse<ExecutiveResponse>> GetExecutiveAsync(ExecutiveQueryParamsRequest queryParams)
        {
            return await _executiveRepository.GetExecutiveAsync(queryParams);
        }
    }
}
