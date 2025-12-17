using Microsoft.AspNetCore.Identity;
using MSINS_API.Models.Request;
using MSINS_API.Models.Response;
using MSINS_API.Repositories.Implementation;
using MSINS_API.Repositories.Interface;
using MSINS_API.Services.Interface;
using SixLabors.ImageSharp;
using System.Net;

namespace MSINS_API.Services.Implementation
{
    public class InitiativeService : IInitiativeService
    {
        private readonly IFileUploadCustomSizeService _fileUploadService;
        private readonly IInitiativeRepository _initiativeRepository;

        private readonly string[] _allowedImageFormats = { "image/jpg", "image/jpeg", "image/png", "image/webp" };
        private const long _maxImageSizeInKb = 100;  // 50 KB
        private const int _requiredWidth = 388;  // Required width
        private const int _requiredHeight = 154; // Required height

        public InitiativeService(IFileUploadCustomSizeService fileUploadService, IInitiativeRepository initiativeRepository)
        {
            _fileUploadService = fileUploadService;
            _initiativeRepository = initiativeRepository;
        }

        public async Task<(int StatusCode, string Message)> AddOrUpdateInitiativeAsync(InitiativeRequest initiativeDto)
        {
            string? fileUrl = null;

            // Handle File Upload (if provided)
            if (initiativeDto.ImageFile == null && (initiativeDto.InitiativeId == null || initiativeDto.InitiativeId.Equals(0)))
            {
                return ((int)HttpStatusCode.BadRequest, $"Please select an image to upload.");
            }
            else if (initiativeDto.ImageFile != null)
            {
                // Validate file format
                if (!_allowedImageFormats.Contains(initiativeDto.ImageFile.ContentType.ToLower()))
                {
                    return ((int)HttpStatusCode.BadRequest, "Invalid file type. Only JPG and PNG are allowed.");
                }

                // Validate file size
                if (initiativeDto.ImageFile.Length > _maxImageSizeInKb * 1024) // Convert KB to Bytes
                {
                    return ((int)HttpStatusCode.BadRequest, $"File size exceeds {_maxImageSizeInKb} KB limit.");
                }

                // Validate exact image dimensions
                using (var stream = initiativeDto.ImageFile.OpenReadStream())
                {
                    using (var image = Image.Load(stream))
                    {
                        if (image.Width != _requiredWidth || image.Height != _requiredHeight)
                        {
                            return ((int)HttpStatusCode.BadRequest,
                                $"Image must be exactly {_requiredWidth}x{_requiredHeight}px. Uploaded image size: {image.Width}x{image.Height}px.");
                        }
                    }
                }

                var uploadResult = await _fileUploadService.UploadFileAsync(initiativeDto.ImageFile, "uploads/initiatives");
                if (!uploadResult.IsSuccess)
                {
                    return ((int)HttpStatusCode.BadRequest, uploadResult.ErrorMessage);
                }
                fileUrl = uploadResult.FileUrl;
            }

            // Call repository (Stored Procedure Execution)
            var (resultCode, message) = await _initiativeRepository.AddOrUpdateInitiativeAsync(initiativeDto, fileUrl);

            if (resultCode.Equals(1))
            {
                return (200, message);
            }
            else
            {
                return (500, "An unexpected error occurred.");
            }
        }

        public async Task<PagedResponse<InitiativeResponse>> GetInitiativeAsync(InitiativeQueryParamsRequest queryParams)
        {
            return await _initiativeRepository.GetInitiativeAsync(queryParams);
        }
    }
}