using MSINS_API.Models.Request;
using MSINS_API.Models.Response;
using MSINS_API.Repositories.Implementation;
using MSINS_API.Repositories.Interface;
using MSINS_API.Services.Interface;
using SixLabors.ImageSharp;
using System.Net;

namespace MSINS_API.Services.Implementation
{
    public class EventService : IEventService
    {
        private readonly IFileUploadCustomSizeService _fileUploadService;
        private readonly IEventRepository _eventRepository;

        private readonly string[] _allowedImageFormats = { "image/jpg", "image/jpeg", "image/png", "image/webp" };
        private const long _maxImageSizeInKb = 1000;  // 1000 KB
        private const int _requiredWidth = 435;  // Required width
        private const int _requiredHeight = 246; // Required height


        public EventService(IFileUploadCustomSizeService fileUploadService, IEventRepository eventRepository)
        {
            _fileUploadService = fileUploadService;
            _eventRepository = eventRepository;
        }
        public async Task<(int StatusCode, string Message)> AddOrUpdateEventAsync(EventRequest EventDto)
        {
            string? fileUrl = null;

            // Handle File Upload (if provided)
            if (EventDto.Image == null && (EventDto.EventId == null || EventDto.EventId.Equals(0)))
            {
                return ((int)HttpStatusCode.BadRequest, $"Please select an image to upload.");
            }
            else if (EventDto.Image != null)
            {
                // Validate file format
                if (!_allowedImageFormats.Contains(EventDto.Image.ContentType.ToLower()))
                {
                    return ((int)HttpStatusCode.BadRequest, "Invalid file type. Only JPG and PNG are allowed.");
                }

                // Validate file size
                if (EventDto.Image.Length > _maxImageSizeInKb * 1024) // Convert KB to Bytes
                {
                    return ((int)HttpStatusCode.BadRequest, $"File size exceeds {_maxImageSizeInKb} KB limit.");
                }

                // Validate exact image dimensions
                using (var stream = EventDto.Image.OpenReadStream())
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

                var uploadResult = await _fileUploadService.UploadFileAsync(EventDto.Image, "uploads/events");
                if (!uploadResult.IsSuccess)
                {
                    return ((int)HttpStatusCode.BadRequest, uploadResult.ErrorMessage);
                }
                fileUrl = uploadResult.FileUrl;
            }
            // Call repository (Stored Procedure Execution)

            var (resultCode, message) = await _eventRepository.AddOrUpdateEventAsync(EventDto, fileUrl);

            if (resultCode.Equals(1))
            {
                return (200, message);
            }
            else
            {
                return (500, "An unexpected error occurred.");
            }
        }

        public async Task<PagedResponse<EventResponse>> GetEventsAsync(EventQueryParamRequest queryParams)
        {
            return await _eventRepository.GetEventsAsync(queryParams);
        }
    }
}
