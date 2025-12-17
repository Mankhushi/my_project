using MSINS_API.Models.Request;
using MSINS_API.Models.Response;
using MSINS_API.Repositories.Implementation;
using MSINS_API.Repositories.Interface;
using MSINS_API.Services.Interface;
using SixLabors.ImageSharp;
using System.Net;

namespace MSINS_API.Services.Implementation
{
    public class PartnerService : IPartnerService
    {
        private readonly IFileUploadCustomSizeService _fileUploadService;
        private readonly IPartnerRepository _partnerRepository;

        private readonly string[] _allowedImageFormats = { "image/jpg", "image/jpeg", "image/png", "image/webp" };
        private const long _maxImageSizeInKb = 100;  // 100 KB
        private const int _requiredWidth = 378;  // Required width
        private const int _requiredHeight = 149; // Required height

        public PartnerService(IFileUploadCustomSizeService fileUploadService, IPartnerRepository partnerRepository)
        {
            _fileUploadService = fileUploadService;
            _partnerRepository = partnerRepository;
        }

        

        public async Task<(int StatusCode, string Message)> AddOrUpdatePartnerAsync(PartnersRequest partnerDto)
        {
            // ✅ Ensure both PartnerLink and LinkType are either provided together or not at all
            if (!string.IsNullOrWhiteSpace(partnerDto.PartnerLink) && partnerDto.LinkType == null)
            {
                return ((int)HttpStatusCode.BadRequest, "LinkType is required when PartnerLink is provided.");
            }
            if (partnerDto.LinkType != null && string.IsNullOrWhiteSpace(partnerDto.PartnerLink))
            {
                return ((int)HttpStatusCode.BadRequest, "PartnerLink is required when LinkType is provided.");
            }

            string? fileUrl = null;

            // Handle File Upload (if provided)
            if (partnerDto.ImageFile == null && (partnerDto.PartnerId == null || partnerDto.PartnerId.Equals(0)))
            {
                return ((int)HttpStatusCode.BadRequest, $"Please select an image to upload.");
            }
            else if (partnerDto.ImageFile != null)
            {
                // Validate file format
                if (!_allowedImageFormats.Contains(partnerDto.ImageFile.ContentType.ToLower()))
                {
                    return ((int)HttpStatusCode.BadRequest, "Invalid file type. Only JPG and PNG are allowed.");
                }

                // Validate file size
                if (partnerDto.ImageFile.Length > _maxImageSizeInKb * 1024) // Convert KB to Bytes
                {
                    return ((int)HttpStatusCode.BadRequest, $"File size exceeds {_maxImageSizeInKb} KB limit.");
                }

                // Validate exact image dimensions
                /*using (var stream = partnerDto.ImageFile.OpenReadStream())
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
                */

                var uploadResult = await _fileUploadService.UploadFileAsync(partnerDto.ImageFile, "uploads/partners");
                if (!uploadResult.IsSuccess)
                {
                    return ((int)HttpStatusCode.BadRequest, uploadResult.ErrorMessage);
                }
                fileUrl = uploadResult.FileUrl;
            }
            // Call repository (Stored Procedure Execution)

            var (resultCode, message) = await _partnerRepository.AddOrUpdatePartnerAsync(partnerDto, fileUrl);

            if (resultCode.Equals(1))
            {
                return (200, message);
            }
            else if (resultCode.Equals(-1))
            {
                return (400, message);
            }
            else
            {
                return (500, "An unexpected error occurred.");
            }
        }

        public async Task<PagedResponse<PartnersResponse>> GetPartnersAsync(PartnersQueryParamsRequest queryParams)
        {
            return await _partnerRepository.GetPartnersAsync(queryParams);
        }
    }
}
