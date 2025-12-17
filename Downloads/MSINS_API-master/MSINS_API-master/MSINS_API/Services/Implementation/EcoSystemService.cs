using MSINS_API.Models.Request;
using MSINS_API.Models.Response;
using MSINS_API.Repositories.Implementation;
using MSINS_API.Repositories.Interface;
using MSINS_API.Services.Interface;
using SixLabors.ImageSharp;
using System.Net;

namespace MSINS_API.Services.Implementation
{
    public class EcoSystemService : IEcoSystemService
    {
        private readonly IFileUploadCustomSizeService _fileUploadService;
        private readonly IEcoSystemRepository _ecosystemRepository;

        private readonly string[] _allowedImageFormats = { "image/jpg", "image/jpeg", "image/png", "image/webp" };
        private const long _maxImageSizeInKb = 50;  // 500 KB
        private const int _requiredWidth = 154;  // Required width
        private const int _requiredHeight = 154; // Required height

        public EcoSystemService(IFileUploadCustomSizeService fileUploadService, IEcoSystemRepository ecosystemRepository)
        {
            _fileUploadService = fileUploadService;
            _ecosystemRepository = ecosystemRepository;
        }

        public async Task<(int Code, string Message)> AddOrUpdateEcoSystemAsync(EcoSystemRequest ecoSystemDto)
        {
            string? fileUrl = null;

            if (ecoSystemDto.ImageFile == null && (ecoSystemDto.EcoSystemId == null || ecoSystemDto.EcoSystemId.Equals(0)))
            {
                return ((int)HttpStatusCode.BadRequest, $"Please select an image to upload.");
            }
            else if (ecoSystemDto.ImageFile != null)
            {
                // Validate file format
                if (!_allowedImageFormats.Contains(ecoSystemDto.ImageFile.ContentType.ToLower()))
                {
                    return ((int)HttpStatusCode.BadRequest, "Invalid file type. Only JPG and PNG are allowed.");
                }

                // Validate file size
                if (ecoSystemDto.ImageFile.Length > _maxImageSizeInKb * 1024) // Convert KB to Bytes
                {
                    return ((int)HttpStatusCode.BadRequest, $"File size exceeds {_maxImageSizeInKb} KB limit.");
                }

                // Validate exact image dimensions
                using (var stream = ecoSystemDto.ImageFile.OpenReadStream())
                {
                    using (var image = Image.Load(stream))  // Image.Load instead of Image.FromStream
                    {
                        if (image.Width != _requiredWidth || image.Height != _requiredHeight)
                        {
                            return ((int)HttpStatusCode.BadRequest,
                                $"Image must be exactly {_requiredWidth}x{_requiredHeight}px. Uploaded image size: {image.Width}x{image.Height}px.");
                        }
                    }
                }

                var uploadResult = await _fileUploadService.UploadFileAsync(ecoSystemDto.ImageFile, "uploads/ecosystem");
                if (!uploadResult.IsSuccess)
                {
                    return ((int)HttpStatusCode.BadRequest, uploadResult.ErrorMessage);
                }
                fileUrl = uploadResult.FileUrl;
            }
            // Call repository (Stored Procedure Execution)

            var (resultCode, message) = await _ecosystemRepository.AddOrUpdateEcoSystemAsync(ecoSystemDto, fileUrl);

            if (resultCode.Equals(1))
            {
                return (200, message);
            }
            else
            {
                return (500, "An unexpected error occurred.");
            }
        }

        public async Task<PagedResponse<EcoSystemResponse>> GetEcoSystemAsync(EcoSystemQueryParamsRequest queryParams)
        {
            return await _ecosystemRepository.GetEcoSystemAsync(queryParams);
        }
    }
}
