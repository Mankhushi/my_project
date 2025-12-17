using MSINS_API.Models.Request;
using MSINS_API.Models.Response;
using MSINS_API.Repositories.Interface;
using MSINS_API.Services.Interface;
using SixLabors.ImageSharp;

namespace MSINS_API.Services.Implementation
{
    public class NewWinnerMasterService : INewWinnerMasterService
    {
        private readonly INewWinnerMasterRepository _repository;
        private readonly IFileUploadCustomSizeService _fileUploadService;

        private readonly string[] _allowedFormats = { "image/jpg", "image/jpeg", "image/png" };
        private const long _maxSize = 500; // KB
        private const int _width = 154;
        private const int _height = 154;

        public NewWinnerMasterService(
            INewWinnerMasterRepository repository,
            IFileUploadCustomSizeService fileUploadService)
        {
            _repository = repository;
            _fileUploadService = fileUploadService;
        }

        // ============================================================
        //                     ADD WINNER
        // ============================================================
        public async Task<(int Code, string Message)> AddWinnerAsync(
            NewWinnerMasterRequest request)
        {
            if (request.SectorId <= 0)
                return (400, "SectorId is required.");

            if (string.IsNullOrWhiteSpace(request.WinnerName))
                return (400, "WinnerName is required.");

            if (request.InitiativeId <= 0)
                return (400, "Valid InitiativeId is required.");

            if (request.WinnerImage == null)
                return (400, "Please upload WinnerImage.");

            // Validate image format
            if (!_allowedFormats.Contains(request.WinnerImage.ContentType.ToLower()))
                return (400, "Only JPG and PNG images are allowed.");

            // Validate size
            if (request.WinnerImage.Length > _maxSize * 1024)
                return (400, $"File size cannot exceed {_maxSize} KB.");

            // Validate image dimensions
            using (var stream = request.WinnerImage.OpenReadStream())
            using (var image = Image.Load(stream))
            {
                if (image.Width != _width || image.Height != _height)
                {
                    return (400,
                        $"Image must be exactly {_width}x{_height}px. " +
                        $"Uploaded: {image.Width}x{image.Height}px.");
                }
            }

            // Upload file
            var upload = await _fileUploadService.UploadFileAsync(
                request.WinnerImage,
                "Initiative/Winner"
            );

            if (!upload.IsSuccess)
                return (400, upload.ErrorMessage);

            // Save to DB
            return await _repository.AddWinnerAsync(
                request,
                upload.FileUrl
            );
        }

        // ============================================================
        //                     UPDATE WINNER
        // ============================================================
        public async Task<(int Code, string Message)> UpdateWinnerAsync(
            int winnerId,
            NewWinnerMasterRequest request)
        {
            if (winnerId <= 0)
                return (400, "Invalid WinnerId.");

            if (request.SectorId <= 0)
                return (400, "SectorId is required.");

            if (string.IsNullOrWhiteSpace(request.WinnerName))
                return (400, "WinnerName is required.");

            if (request.InitiativeId <= 0)
                return (400, "Valid InitiativeId is required.");

            string? fileUrl = null;

            // If new image uploaded
            if (request.WinnerImage != null)
            {
                if (!_allowedFormats.Contains(request.WinnerImage.ContentType.ToLower()))
                    return (400, "Only JPG and PNG allowed.");

                if (request.WinnerImage.Length > _maxSize * 1024)
                    return (400, $"File size cannot exceed {_maxSize} KB.");

                using (var stream = request.WinnerImage.OpenReadStream())
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
                    request.WinnerImage,
                    "Initiative/Winner"
                );

                if (!upload.IsSuccess)
                    return (400, upload.ErrorMessage);

                fileUrl = upload.FileUrl;
            }

            return await _repository.UpdateWinnerAsync(
                winnerId,
                request,
                fileUrl
            );
        }

        // ============================================================
        //                     GET ALL WINNERS
        // ============================================================
        public async Task<List<NewWinnerMasterResponse>> GetWinnersAsync(
            int? sectorId,
            bool? isActive,
            int? initiativeId)
        {
            return await _repository.GetWinnersAsync(sectorId, isActive, initiativeId);
        }

        // ============================================================
        //                     GET WINNER BY ID
        // ============================================================
        public async Task<NewWinnerMasterResponse?> GetWinnerByIdAsync(int winnerId)
        {
            if (winnerId <= 0)
                return null;

            return await _repository.GetWinnerByIdAsync(winnerId);
        }
    }
}
