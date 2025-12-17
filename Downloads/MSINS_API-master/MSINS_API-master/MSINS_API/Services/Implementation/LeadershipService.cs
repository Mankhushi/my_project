using MSINS_API.Models.Request;
using MSINS_API.Models.Response;
using MSINS_API.Repositories.Implementation;
using MSINS_API.Repositories.Interface;
using MSINS_API.Services.Interface;
using SixLabors.ImageSharp;
using System.Net;

namespace MSINS_API.Services.Implementation
{
    public class LeadershipService : ILeadershipService
    {
        private readonly IFileUploadCustomSizeService _fileUploadService;
        private readonly ILeadershipRepository _leaderRepository;

        private readonly string[] _allowedImageFormats = { "image/jpg", "image/jpeg", "image/png", "image/webp" };
        private const long _maxImageSizeInKb = 200;  // 200 KB
        private const int _requiredWidth = 400;  // Required width
        private const int _requiredHeight = 400; // Required height

        public LeadershipService(IFileUploadCustomSizeService fileUploadService, ILeadershipRepository leaderRepository)
        {
            _fileUploadService = fileUploadService;
            _leaderRepository = leaderRepository;
        }

        public async Task<(int StatusCode, string Message)> AddOrUpdateLeadershipAsync(LeadershipRequest leaderDto)
        {
            string? fileUrl = null;

            // Handle File Upload (if provided)
            if (leaderDto.ImageFile == null && (leaderDto.LeadershipId == null || leaderDto.LeadershipId.Equals(0)))
            {
                return ((int)HttpStatusCode.BadRequest, $"Please select an image to upload.");
            }
            else if (leaderDto.ImageFile != null)
            {
                // Validate file format
                if (!_allowedImageFormats.Contains(leaderDto.ImageFile.ContentType.ToLower()))
                {
                    return ((int)HttpStatusCode.BadRequest, "Invalid file type. Only JPG and PNG are allowed.");
                }

                // Validate file size
                if (leaderDto.ImageFile.Length > _maxImageSizeInKb * 1024) // Convert KB to Bytes
                {
                    return ((int)HttpStatusCode.BadRequest, $"File size exceeds {_maxImageSizeInKb} KB limit.");
                }

                // Validate exact image dimensions
                using (var stream = leaderDto.ImageFile.OpenReadStream())
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

                var uploadResult = await _fileUploadService.UploadFileAsync(leaderDto.ImageFile, "uploads/leaders");
                if (!uploadResult.IsSuccess)
                {
                    return ((int)HttpStatusCode.BadRequest, uploadResult.ErrorMessage);
                }
                fileUrl = uploadResult.FileUrl;
            }
            // Call repository (Stored Procedure Execution)

            var (resultCode, message) = await _leaderRepository.AddOrUpdateLeadershipAsync(leaderDto, fileUrl);

            if (resultCode.Equals(1))
            {
                return (200, message);
            }
            else
            {
                return (500, "An unexpected error occurred.");
            }
        }

        public async Task<PagedResponse<LeadershipResponse>> GetLeadersAsync(LeaderQueryParamsRequest queryParams)
        {
            return await _leaderRepository.GetLeadersAsync(queryParams);
        }
    }
}
