using MSINS_API.Models.Request;
using MSINS_API.Models.Response;
using MSINS_API.Repositories.Interface;
using MSINS_API.Services.Interface;
using SixLabors.ImageSharp;
using System.Net;


namespace MSINS_API.Services.Implementation
{
    public class BannerService : IBannerService
    {
        private readonly IFileUploadCustomSizeService _fileUploadService;
        private readonly IBannerRepository _bannerRepository;

        private readonly string[] _allowedImageFormats = { "image/jpg", "image/jpeg", "image/png", "image/webp" };
        private const long _maxImageSizeInKb = 1000;  // 1000 KB
        private const int _requiredWidth = 3456;  // Required width
        private const int _requiredHeight = 1728; // Required height


        public BannerService(IFileUploadCustomSizeService fileUploadService, IBannerRepository bannerRepository)
        {
            _fileUploadService = fileUploadService;
            _bannerRepository = bannerRepository;
        }

        public async Task<(int StatusCode, string Message)> AddOrUpdateBannerAsync(BannerRequest bannerDto)
        {
            string? fileUrl = null;

            // Handle File Upload (if provided)
            if (bannerDto.ImageFile == null && (bannerDto.BannerId == null || bannerDto.BannerId.Equals(0)))
            {
                return ((int)HttpStatusCode.BadRequest, $"Please select an image to upload.");
            }
            else if (bannerDto.ImageFile != null)
            {
                // Validate file format
                if (!_allowedImageFormats.Contains(bannerDto.ImageFile.ContentType.ToLower()))
                {
                    return ((int)HttpStatusCode.BadRequest, "Invalid file type. Only JPG and PNG are allowed.");
                }

                // Validate file size
                if (bannerDto.ImageFile.Length > _maxImageSizeInKb * 1024) // Convert KB to Bytes
                {
                    return ((int)HttpStatusCode.BadRequest, $"File size exceeds {_maxImageSizeInKb} KB limit.");
                }

                // Validate exact image dimensions
                using (var stream = bannerDto.ImageFile.OpenReadStream())
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

                var uploadResult = await _fileUploadService.UploadFileAsync(bannerDto.ImageFile, "uploads/banners");
                if (!uploadResult.IsSuccess)
                {
                    return ((int)HttpStatusCode.BadRequest, uploadResult.ErrorMessage);
                }
                fileUrl = uploadResult.FileUrl;
            }
            // Call repository (Stored Procedure Execution)

            var (resultCode, message) = await _bannerRepository.AddOrUpdateBannerAsync(bannerDto, fileUrl);

            if (resultCode.Equals(1))
            {
                return (200, message);
            }
            else
            {
                return (500, "An unexpected error occurred.");
            }

        }

        public async Task<PagedResponse<BannerResponse>> GetBannersAsync(BannerQueryParamsRequest queryParams)
        {
            return await _bannerRepository.GetBannersAsync(queryParams);
        }
    }
}

