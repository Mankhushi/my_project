using Microsoft.AspNetCore.Http;
using MSINS_API.Models.Request;
using MSINS_API.Models.Response;
using MSINS_API.Repositories.Interface;
using MSINS_API.Services.Interface;
using SixLabors.ImageSharp;
using System.Net;

namespace MSINS_API.Services.Implementation
{
    public class NewInitiativeMasterService : INewInitiativeMasterService
    {
        private readonly IFileUploadCustomSizeService _fileUploadService;
        private readonly INewInitiativeMasterRepository _initiativeRepository;

        private readonly string[] _allowedImageFormats = { "image/jpg", "image/jpeg", "image/png", "image/webp" };
        private const long _maxImageSizeInKb = 500;
        private const int _requiredWidth = 1200;
        private const int _requiredHeight = 400;

        public NewInitiativeMasterService(
            IFileUploadCustomSizeService fileUploadService,
            INewInitiativeMasterRepository initiativeRepository)
        {
            _fileUploadService = fileUploadService;
            _initiativeRepository = initiativeRepository;
        }

        // -----------------------------------------------------------------------
        // ⭐ ADD INITIATIVE
        // -----------------------------------------------------------------------
        public async Task<(int Code, string Message)> AddInitiativeMasterAsync(NewInitiativeMasterRequest request)
        {
            string? fileUrl = null;

            if (request.HeaderImageFile == null)
            {
                return ((int)HttpStatusCode.BadRequest, "Please upload header image.");
            }

            // Validate image type
            if (!_allowedImageFormats.Contains(request.HeaderImageFile.ContentType.ToLower()))
            {
                return ((int)HttpStatusCode.BadRequest, "Invalid file type. Only JPG, JPEG, PNG, WEBP allowed.");
            }

            // Validate size
            if (request.HeaderImageFile.Length > _maxImageSizeInKb * 1024)
                return ((int)HttpStatusCode.BadRequest, $"File size exceeds limit ({_maxImageSizeInKb} KB).");

            // Validate dimensions
            using (var stream = request.HeaderImageFile.OpenReadStream())
            using (var image = Image.Load(stream))
            {
                if (image.Width != _requiredWidth || image.Height != _requiredHeight)
                {
                    return ((int)HttpStatusCode.BadRequest,
                        $"Image must be exactly {_requiredWidth}x{_requiredHeight}px. Uploaded: {image.Width}x{image.Height}px.");
                }
            }

            // Upload File
            var uploadResult = await _fileUploadService.UploadFileAsync(request.HeaderImageFile, "uploads/initiative");
            if (!uploadResult.IsSuccess)
                return (400, uploadResult.ErrorMessage);

            fileUrl = uploadResult.FileUrl;

            // Call Repository
            var (code, message) = await _initiativeRepository.AddInitiativeMasterAsync(request, fileUrl);

            // Stored Procedure returns: 200 → Success
            if (code == 200)
                return (200, message);

            return (500, message);
        }

        // -----------------------------------------------------------------------
        // ⭐ UPDATE INITIATIVE
        // -----------------------------------------------------------------------
        public async Task<(int Code, string Message)> UpdateInitiativeMasterAsync(int id, NewInitiativeMasterRequest request)
        {
            string? fileUrl = null;

            // If user uploaded new image
            if (request.HeaderImageFile != null)
            {
                if (!_allowedImageFormats.Contains(request.HeaderImageFile.ContentType.ToLower()))
                    return (400, "Invalid file type. Only JPG, JPEG, PNG, WEBP allowed.");

                if (request.HeaderImageFile.Length > _maxImageSizeInKb * 1024)
                    return (400, $"File size exceeds limit ({_maxImageSizeInKb} KB).");

                using (var stream = request.HeaderImageFile.OpenReadStream())
                using (var image = Image.Load(stream))
                {
                    if (image.Width != _requiredWidth || image.Height != _requiredHeight)
                    {
                        return (400,
                            $"Image must be exactly {_requiredWidth}x{_requiredHeight}px. Uploaded: {image.Width}x{image.Height}px.");
                    }
                }

                var uploadResult = await _fileUploadService.UploadFileAsync(request.HeaderImageFile, "uploads/initiative");
                if (!uploadResult.IsSuccess)
                    return (400, uploadResult.ErrorMessage);

                fileUrl = uploadResult.FileUrl;
            }

            var (code, message) = await _initiativeRepository.UpdateInitiativeMasterAsync(id, request, fileUrl);

            if (code == 200)
                return (200, message);

            return (500, message);
        }

        // -----------------------------------------------------------------------
        // ⭐ GET BY ID
        // -----------------------------------------------------------------------
        public async Task<NewInitiativeMasterResponse?> GetInitiativeMasterByIdAsync(int id)
        {
            return await _initiativeRepository.GetInitiativeMasterByIdAsync(id);
        }

        // -----------------------------------------------------------------------
        // ⭐ GET ALL (Pagination + Search)
        // -----------------------------------------------------------------------
        public async Task<(List<NewInitiativeMasterResponse> Data, int TotalRecords)>
            GetInitiativeMasterAsync(string? title, bool? isActive, int pageIndex, int pageSize)
        {
            return await _initiativeRepository.GetInitiativeMasterAsync(title, isActive, pageIndex, pageSize);
        }
    }
}
