using MSINS_API.Models.Request;
using MSINS_API.Models.Response;
using MSINS_API.Repositories.Interface;
using MSINS_API.Services.Interface;
using System.Net;

namespace MSINS_API.Services.Implementation
{
    public class NewInitiativeSpeakerService : INewInitiativeSpeakerService
    {
        private readonly INewInitiativeSpeakerRepository _speakerRepository;
        private readonly IFileUploadCustomSizeService _fileUploadService;

        public NewInitiativeSpeakerService(
            INewInitiativeSpeakerRepository speakerRepository,
            IFileUploadCustomSizeService fileUploadService)
        {
            _speakerRepository = speakerRepository;
            _fileUploadService = fileUploadService;
        }

        // --------------------- ADD SPEAKER ---------------------
        public async Task<(int Code, string Message)> AddSpeakerAsync(NewInitiativeSpeakerRequest request)
        {
            if (request.ProfilePicUrl == null)
                return (400, "Please upload profile picture.");

            string[] allowedFormats = { "image/jpg", "image/jpeg", "image/png", "image/webp" };
            if (!allowedFormats.Contains(request.ProfilePicUrl.ContentType.ToLower()))
                return (400, "Invalid file type. Only JPG, PNG, WEBP allowed.");

            var upload = await _fileUploadService.UploadFileAsync(request.ProfilePicUrl, "uploads/speakers");
            if (!upload.IsSuccess)
                return (400, upload.ErrorMessage);

            string fileUrl = upload.FileUrl;

            var (resultCode, message) = await _speakerRepository.AddSpeakerAsync(request, fileUrl);

            return resultCode == 1 ? (200, message) : (500, "Unexpected error occurred.");
        }


        // --------------------- UPDATE SPEAKER ---------------------
        public async Task<(int Code, string Message)> UpdateSpeakerAsync(int speakerId, NewInitiativeSpeakerRequest request)
        {
            if (speakerId <= 0)
                return (400, "Invalid Speaker Id.");

            string? fileUrl = null;

            if (request.ProfilePicUrl != null)
            {
                var upload = await _fileUploadService.UploadFileAsync(request.ProfilePicUrl, "uploads/speakers");
                if (!upload.IsSuccess)
                    return (400, upload.ErrorMessage);

                fileUrl = upload.FileUrl;
            }

            var (resultCode, message) = await _speakerRepository.UpdateSpeakerAsync(speakerId, request, fileUrl);

            return resultCode == 1 ? (200, message) : (500, "Unexpected error occurred.");
        }


        // --------------------- GET BY ID ---------------------
        public async Task<NewInitiativeSpeakerResponse?> GetSpeakerByIdAsync(int speakerId)
        {
            return await _speakerRepository.GetSpeakerByIdAsync(speakerId);
        }

        // --------------------- GET ALL (Pagination + Filters) ---------------------
        public async Task<PagedResponse<NewInitiativeSpeakerResponse>> GetAllSpeakersAsync(string? name, string? designation, bool? isActive, int pageIndex, int pageSize)
        {
            return await _speakerRepository.GetAllSpeakersAsync(name, designation, isActive, pageIndex, pageSize
            );
        }

    }
}
