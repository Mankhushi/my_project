using MSINS_API.Models.Request;
using MSINS_API.Models.Response;
using MSINS_API.Repositories.Interface;
using MSINS_API.Services.Interface;
using SixLabors.ImageSharp;

namespace MSINS_API.Services.Implementation
{
    public class NewPhotographsMasterService : INewPhotographsMasterService
    {
        private readonly INewPhotographsMasterRepository _repository;
        private readonly IFileUploadCustomSizeService _fileUploadService;

        private readonly string[] _allowedFormats = { "image/jpg", "image/jpeg", "image/png" };
        private const long _maxSize = 500; // KB
        private const int _width = 154;
        private const int _height = 154;

        public NewPhotographsMasterService(
            INewPhotographsMasterRepository repository,
            IFileUploadCustomSizeService fileUploadService)
        {
            _repository = repository;
            _fileUploadService = fileUploadService;
        }

        // ============================================================
        //                        ADD PHOTOGRAPH
        // ============================================================
        public async Task<(int Code, string Message)> AddPhotographAsync(
            NewPhotographsMasterRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.PhotographName))
                return (400, "Photograph Name is required.");

            if (request.InitiativeId <= 0)
                return (400, "Valid InitiativeId is required.");

            if (request.PhotographFile == null)
                return (400, "Please upload a photograph.");

            // Validate file type
            if (!_allowedFormats.Contains(request.PhotographFile.ContentType.ToLower()))
                return (400, "Only JPG and PNG images are allowed.");

            // Validate file size
            if (request.PhotographFile.Length > _maxSize * 1024)
                return (400, $"File size cannot exceed {_maxSize} KB.");

            // Validate dimensions
            using (var stream = request.PhotographFile.OpenReadStream())
            using (var image = Image.Load(stream))
            {
                if (image.Width != _width || image.Height != _height)
                {
                    return (400,
                        $"Image must be exactly {_width}x{_height}px. " +
                        $"Uploaded size: {image.Width}x{image.Height}px.");
                }
            }

            // Upload file
            var uploadResult = await _fileUploadService.UploadFileAsync(
                request.PhotographFile,
                "Initiative/Photographs"
            );

            if (!uploadResult.IsSuccess)
                return (400, uploadResult.ErrorMessage);

            // Save in DB
            return await _repository.AddPhotographAsync(
                request,
                uploadResult.FileUrl
            );
        }

        // ============================================================
        //                        UPDATE PHOTOGRAPH
        // ============================================================
        public async Task<(int Code, string Message)> UpdatePhotographAsync(
            int photographId,
            NewPhotographsMasterRequest request)
        {
            if (photographId <= 0)
                return (400, "Invalid PhotographId.");

            if (string.IsNullOrWhiteSpace(request.PhotographName))
                return (400, "Photograph Name is required.");

            if (request.InitiativeId <= 0)
                return (400, "Valid InitiativeId is required.");

            string? fileUrl = null;

            // If new file is uploaded
            if (request.PhotographFile != null)
            {
                if (!_allowedFormats.Contains(request.PhotographFile.ContentType.ToLower()))
                    return (400, "Only JPG and PNG are allowed.");

                if (request.PhotographFile.Length > _maxSize * 1024)
                    return (400, $"File size cannot exceed {_maxSize} KB.");

                using (var stream = request.PhotographFile.OpenReadStream())
                using (var image = Image.Load(stream))
                {
                    if (image.Width != _width || image.Height != _height)
                    {
                        return (400,
                            $"Image must be {_width}x{_height}px. " +
                            $"Uploaded: {image.Width}x{image.Height}px.");
                    }
                }

                var upload = await _fileUploadService.UploadFileAsync(
                    request.PhotographFile,
                    "Initiative/Photographs"
                );

                if (!upload.IsSuccess)
                    return (400, upload.ErrorMessage);

                fileUrl = upload.FileUrl;
            }

            return await _repository.UpdatePhotographAsync(
                photographId,
                request,
                fileUrl
            );
        }

        // ============================================================
        //                        GET ALL
        // ============================================================
        public async Task<List<NewPhotographsMasterResponse>> GetPhotographsAsync(
            int? initiativeId,
            bool? isActive)
        {
            return await _repository.GetPhotographsAsync(initiativeId, isActive);
        }

        // ============================================================
        //                        GET BY ID
        // ============================================================
        public async Task<NewPhotographsMasterResponse?> GetPhotographByIdAsync(
            int photographId)
        {
            if (photographId <= 0)
                return null;

            return await _repository.GetPhotographByIdAsync(photographId);
        }
    }
}
