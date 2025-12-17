using MSINS_API.Models.Request;
using MSINS_API.Models.Response;
using MSINS_API.Repositories.Interface;
using MSINS_API.Services.Interface;
using SixLabors.ImageSharp;
using System.Net;

namespace MSINS_API.Services.Implementation
{
    public class MediaService : IMediaService
    {
        private readonly IFileUploadCustomSizeService _fileUploadService;
        private readonly IMediaRepository _MediaRepository;

        private readonly string[] _allowedImageFormats = { "image/jpg", "image/jpeg", "image/png", "image/webp" };
        private const long _maxImageSizeInKb = 1000;  // 1000 KB
        private const int _requiredWidth = 274;  // Required width
        private const int _requiredHeight = 184; // Required height


        public MediaService(IFileUploadCustomSizeService fileUploadService, IMediaRepository MediaRepository)
        {
            _fileUploadService = fileUploadService;
            _MediaRepository = MediaRepository;
        }
        public async Task<(int StatusCode, string Message)> AddOrUpdateMediaAsync(MediaRequest MediaDto)
        {
            string? fileUrl = null;

            // Handle File Upload (if provided)
            if (MediaDto.Image == null && (MediaDto.MediaId == null || MediaDto.MediaId.Equals(0)))
            {
                return ((int)HttpStatusCode.BadRequest, $"Please select an image to upload.");
            }
            else if (MediaDto.Image != null)
            {
                // Validate file format
                if (!_allowedImageFormats.Contains(MediaDto.Image.ContentType.ToLower()))
                {
                    return ((int)HttpStatusCode.BadRequest, "Invalid file type. Only JPG and PNG are allowed.");
                }

                // Validate file size
                if (MediaDto.Image.Length > _maxImageSizeInKb * 1024) // Convert KB to Bytes
                {
                    return ((int)HttpStatusCode.BadRequest, $"File size exceeds {_maxImageSizeInKb} KB limit.");
                }

                // Validate exact image dimensions
                using (var stream = MediaDto.Image.OpenReadStream())
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

                var uploadResult = await _fileUploadService.UploadFileAsync(MediaDto.Image, "uploads/media");
                if (!uploadResult.IsSuccess)
                {
                    return ((int)HttpStatusCode.BadRequest, uploadResult.ErrorMessage);
                }
                fileUrl = uploadResult.FileUrl;
            }
            // Call repository (Stored Procedure Execution)

            var (resultCode, message) = await _MediaRepository.AddOrUpdateMediaAsync(MediaDto, fileUrl);

            if (resultCode.Equals(1))
            {
                return (200, message);
            }
            else
            {
                return (500, "An unexpected error occurred.");
            }
        }

        public async Task<PagedResponse<MediaResponse>> GetMediasAsync(MediaQueryParamRequest queryParams)
        {
            return await _MediaRepository.GetMediasAsync(queryParams);
        }
    }
}
