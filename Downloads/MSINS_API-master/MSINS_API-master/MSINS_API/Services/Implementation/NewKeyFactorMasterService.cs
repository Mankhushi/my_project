using MSINS_API.Models.Request;
using MSINS_API.Models.Response;
using MSINS_API.Repositories.Interface;
using MSINS_API.Services.Interface;
using SixLabors.ImageSharp;

namespace MSINS_API.Services.Implementation
{
    public class NewKeyFactorMasterService : INewKeyFactorMasterService
    {
        private readonly INewKeyFactorMasterRepository _repository;
        private readonly IFileUploadCustomSizeService _fileUploadService;

        private readonly string[] _allowedFormats = { "image/jpg", "image/jpeg", "image/png" };
        private const long _maxSize = 500; // KB
        private const int _width = 154;
        private const int _height = 154;

        public NewKeyFactorMasterService(
            INewKeyFactorMasterRepository repository,
            IFileUploadCustomSizeService fileUploadService)
        {
            _repository = repository;
            _fileUploadService = fileUploadService;
        }

        // ============================================================
        //                        ADD KEY FACTOR
        // ============================================================
        public async Task<(int Code, string Message)> AddKeyFactorAsync(
            NewKeyFactorMasterRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.KeyFactorName))
                return (400, "KeyFactorName is required.");

            if (request.InitiativeId <= 0)
                return (400, "Valid InitiativeId is required.");

            if (request.KeyFactorImage == null)
                return (400, "Please upload KeyFactorImage.");

            // Validate file type
            if (!_allowedFormats.Contains(request.KeyFactorImage.ContentType.ToLower()))
                return (400, "Only JPG and PNG images are allowed.");

            // Validate size
            if (request.KeyFactorImage.Length > _maxSize * 1024)
                return (400, $"File size cannot exceed {_maxSize} KB.");

            // Validate image dimensions
            using (var stream = request.KeyFactorImage.OpenReadStream())
            using (var image = Image.Load(stream))
            {
                if (image.Width != _width || image.Height != _height)
                {
                    return (400,
                        $"Image must be exactly {_width}x{_height}px. " +
                        $"Uploaded: {image.Width}x{image.Height}px.");
                }
            }

            // Upload image
            var upload = await _fileUploadService.UploadFileAsync(
                request.KeyFactorImage,
                "Initiative/KeyFactor"
            );

            if (!upload.IsSuccess)
                return (400, upload.ErrorMessage);

            // Save to database
            return await _repository.AddKeyFactorAsync(
                request,
                upload.FileUrl
            );
        }

        // ============================================================
        //                        UPDATE KEY FACTOR
        // ============================================================
        public async Task<(int Code, string Message)> UpdateKeyFactorAsync(
            int keyFactorId,
            NewKeyFactorMasterRequest request)
        {
            if (keyFactorId <= 0)
                return (400, "Invalid KeyFactorId.");

            if (string.IsNullOrWhiteSpace(request.KeyFactorName))
                return (400, "KeyFactorName is required.");

            if (request.InitiativeId <= 0)
                return (400, "Valid InitiativeId is required.");

            string? fileUrl = null;

            // If new image uploaded
            if (request.KeyFactorImage != null)
            {
                if (!_allowedFormats.Contains(request.KeyFactorImage.ContentType.ToLower()))
                    return (400, "Only JPG and PNG allowed.");

                if (request.KeyFactorImage.Length > _maxSize * 1024)
                    return (400, $"File size cannot exceed {_maxSize} KB.");

                using (var stream = request.KeyFactorImage.OpenReadStream())
                using (var image = Image.Load(stream))
                {
                    if (image.Width != _width || image.Height != _height)
                    {
                        return (400,
                            $"Image must be {_width}x{_height}px. " +
                            $"Uploaded: {image.Width}x{image.Height}px.");
                    }
                }

                // Upload new image
                var upload = await _fileUploadService.UploadFileAsync(
                    request.KeyFactorImage,
                    "Initiative/KeyFactor"
                );

                if (!upload.IsSuccess)
                    return (400, upload.ErrorMessage);

                fileUrl = upload.FileUrl;
            }

            return await _repository.UpdateKeyFactorAsync(
                keyFactorId,
                request,
                fileUrl
            );
        }

        // ============================================================
        //                        GET ALL
        // ============================================================
        public async Task<List<NewKeyFactorMasterResponse>> GetKeyFactorsAsync(
            bool? isActive,
            int? initiativeId)
        {
            return await _repository.GetKeyFactorsAsync(isActive, initiativeId);
        }

        // ============================================================
        //                        GET BY ID
        // ============================================================
        public async Task<NewKeyFactorMasterResponse?> GetKeyFactorByIdAsync(
            int keyFactorId)
        {
            if (keyFactorId <= 0)
                return null;

            return await _repository.GetKeyFactorByIdAsync(keyFactorId);
        }
    }
}
