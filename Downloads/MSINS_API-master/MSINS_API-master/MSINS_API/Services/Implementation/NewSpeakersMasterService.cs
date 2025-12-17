using MSINS_API.Models.Request;
using MSINS_API.Models.Response;
using MSINS_API.Repositories.Interface;
using MSINS_API.Services.Interface;
using SixLabors.ImageSharp;
using System.Net;

namespace MSINS_API.Services.Implementation
{
    public class NewSpeakersMasterService : INewSpeakersMasterService
    {
        private readonly INewSpeakersMasterRepository _speakerRepository;
        private readonly IFileUploadCustomSizeService _fileUploadService;

        // ✅ Allowed file formats
        private readonly string[] _allowedImageFormats = { "image/jpg", "image/jpeg", "image/png", "image/webp" };

        // ✅ File validation settings
        private const long _maxImageSizeInKb = 500;  // 500 KB limit
        private const int _requiredWidth = 300;      // Expected width (optional)
        private const int _requiredHeight = 300;     // Expected height (optional)

        public NewSpeakersMasterService(
            INewSpeakersMasterRepository speakerRepository,
            IFileUploadCustomSizeService fileUploadService)
        {
            _speakerRepository = speakerRepository;
            _fileUploadService = fileUploadService;
        }

        // ADD SPEAKER  ------------------------------------------>>>
        public async Task<(int Code, string Message)> AddSpeakerAsync(NewSpeakersMasterRequest model)
        {
            string? fileUrl = null;

            // Step 1: Validate Image File
            if (model.ProfilePic == null)
            {
                return ((int)HttpStatusCode.BadRequest, "Please select a profile picture to upload.");
            }

            // Validate file type
            if (!_allowedImageFormats.Contains(model.ProfilePic.ContentType.ToLower()))
            {
                return ((int)HttpStatusCode.BadRequest, "Invalid image type. Only JPG, PNG, or WEBP allowed.");
            }

            // Validate file size
            if (model.ProfilePic.Length > _maxImageSizeInKb * 1024)
            {
                return ((int)HttpStatusCode.BadRequest, $"Image exceeds {_maxImageSizeInKb} KB size limit.");
            }

            // (Optional) Validate image dimensions
            using (var stream = model.ProfilePic.OpenReadStream())
            {
                using (var image = Image.Load(stream))
                {
                    if (image.Width < _requiredWidth / 2 || image.Height < _requiredHeight / 2)
                    {
                        return ((int)HttpStatusCode.BadRequest,
                            $"Image is too small. Minimum size should be around {_requiredWidth}x{_requiredHeight}px.");
                    }
                }
            }

            // Step 2: Upload File
            var uploadResult = await _fileUploadService.UploadFileAsync(model.ProfilePic, "uploads/speakers");
            if (!uploadResult.IsSuccess)
            {
                return ((int)HttpStatusCode.BadRequest, uploadResult.ErrorMessage);
            }
            fileUrl = uploadResult.FileUrl;

            // Step 3: Call Repository
            var (resultCode, message) = await _speakerRepository.AddSpeakerAsync(model, fileUrl);

            // Step 4: Handle Response
            if (resultCode == 1)
            {
                return (200, "Speaker added successfully.");
            }

            return (500, message ?? "An unexpected error occurred while adding the speaker.");
        }

        // UPDATE SPEAKER ----------------------------------------->>>>>
        public async Task<(int Code, string Message)> UpdateSpeakerAsync(int id, NewSpeakersMasterRequest model)
        {
            string? fileUrl = null;

            // Step 1: Handle Optional Image Upload
            if (model.ProfilePic != null)
            {
                // Validate file type
                if (!_allowedImageFormats.Contains(model.ProfilePic.ContentType.ToLower()))
                {
                    return ((int)HttpStatusCode.BadRequest, "Invalid image type. Only JPG, PNG, or WEBP allowed.");
                }

                // Validate file size
                if (model.ProfilePic.Length > _maxImageSizeInKb * 1024)
                {
                    return ((int)HttpStatusCode.BadRequest, $"Image exceeds {_maxImageSizeInKb} KB size limit.");
                }

                // (Optional) Validate image dimensions
                using (var stream = model.ProfilePic.OpenReadStream())
                {
                    using (var image = Image.Load(stream))
                    {
                        if (image.Width < _requiredWidth / 2 || image.Height < _requiredHeight / 2)
                        {
                            return ((int)HttpStatusCode.BadRequest,
                                $"Image is too small. Minimum size should be around {_requiredWidth}x{_requiredHeight}px.");
                        }
                    }
                }

                //  Upload New Image
                var uploadResult = await _fileUploadService.UploadFileAsync(model.ProfilePic, "uploads/speakers");
                if (!uploadResult.IsSuccess)
                {
                    return ((int)HttpStatusCode.BadRequest, uploadResult.ErrorMessage);
                }

                fileUrl = uploadResult.FileUrl;
            }

            // Step 2: Call Repository
            var (resultCode, message) = await _speakerRepository.UpdateSpeakerAsync(id, model, fileUrl);

            //  Step 3: Handle Response
            if (resultCode == 1)
            {
                return (200, "Speaker updated successfully.");
            }

            return (500, message ?? "An unexpected error occurred while updating the speaker.");
        }

        // GET BY ID  ----------------------------------------------------->>
        public async Task<(int Code, string Message, NewSpeakerMasterResponse? Data)> GetSpeakerByIdAsync(int id)
        {
            var result = await _speakerRepository.GetSpeakerByIdAsync(id);
            if (result == null)
                return ((int)HttpStatusCode.NotFound, "Speaker not found.", null);

            return ((int)HttpStatusCode.OK, "Speaker fetched successfully.", result);
        }

        // GET ALL SPEAKERS ---------------------------------------------------->>
        public async Task<PagedResponse<NewSpeakerMasterResponse>> GetAllSpeakersAsync(int pageNumber, int pageSize, string? searchTerm = null)
        {
        // Pass search term to repository
            return await _speakerRepository.GetAllSpeakersAsync(pageNumber, pageSize, searchTerm);
        }

    }
}
