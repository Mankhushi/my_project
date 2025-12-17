using MSINS_API.Models.Request;
using MSINS_API.Models.Response;
using MSINS_API.Repositories.Interface;
using MSINS_API.Services.Interface;
using SixLabors.ImageSharp;

namespace MSINS_API.Services.Implementation
{
    public class NewSectorOpprtunityMasterService : INewSectorOpprtunityMasterService
    {
        private readonly INewSectorOpprtunityMasterRepository _repository;
        private readonly IFileUploadCustomSizeService _fileUploadService;

        private readonly string[] _allowedFormats = { "image/jpg", "image/jpeg", "image/png" };
        private const long _maxSize = 500;      // KB
        private const int _width = 154;
        private const int _height = 154;

        public NewSectorOpprtunityMasterService(
            INewSectorOpprtunityMasterRepository repository,
            IFileUploadCustomSizeService fileUploadService)
        {
            _repository = repository;
            _fileUploadService = fileUploadService;
        }

        // ============================================================
        //                      ADD SECTOR OPPORTUNITY
        // ============================================================
        public async Task<(int Code, string Message)> AddSectorOpprtunityAsync(
            NewSectorOpprtunityMasterRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.SectorOpprtunityName))
                return (400, "SectorOpprtunityName is required.");

            if (request.InitiativeId <= 0)
                return (400, "Valid InitiativeId is required.");

            if (request.SectorOpprtunityImage == null)
                return (400, "Please upload SectorOpprtunityImage.");

            // Validate file format
            if (!_allowedFormats.Contains(request.SectorOpprtunityImage.ContentType.ToLower()))
                return (400, "Only JPG and PNG images are allowed.");

            // Validate file size
            if (request.SectorOpprtunityImage.Length > _maxSize * 1024)
                return (400, $"File size cannot exceed {_maxSize} KB.");

            // Validate image dimensions
            using (var stream = request.SectorOpprtunityImage.OpenReadStream())
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
                request.SectorOpprtunityImage,
                "Initiative/SectorOpprtunity"
            );

            if (!upload.IsSuccess)
                return (400, upload.ErrorMessage);

            // Save DB
            return await _repository.AddSectorOpprtunityAsync(
                request,
                upload.FileUrl
            );
        }

        // ============================================================
        //                     UPDATE SECTOR OPPORTUNITY
        // ============================================================
        public async Task<(int Code, string Message)> UpdateSectorOpprtunityAsync(
            int sectorOpprtunityId,
            NewSectorOpprtunityMasterRequest request)
        {
            if (sectorOpprtunityId <= 0)
                return (400, "Invalid SectorOpprtunityId.");

            if (string.IsNullOrWhiteSpace(request.SectorOpprtunityName))
                return (400, "SectorOpprtunityName is required.");

            if (request.InitiativeId <= 0)
                return (400, "Valid InitiativeId is required.");

            string? fileUrl = null;

            // Check if new image uploaded
            if (request.SectorOpprtunityImage != null)
            {
                if (!_allowedFormats.Contains(request.SectorOpprtunityImage.ContentType.ToLower()))
                    return (400, "Only JPG and PNG allowed.");

                if (request.SectorOpprtunityImage.Length > _maxSize * 1024)
                    return (400, $"File size cannot exceed {_maxSize} KB.");

                using (var stream = request.SectorOpprtunityImage.OpenReadStream())
                using (var image = Image.Load(stream))
                {
                    if (image.Width != _width || image.Height != _height)
                    {
                        return (400,
                            $"Image must be {_width}x{_height}px. " +
                            $"Uploaded: {image.Width}x{image.Height}px.");
                    }
                }

                // Upload new file
                var upload = await _fileUploadService.UploadFileAsync(
                    request.SectorOpprtunityImage,
                    "Initiative/SectorOpprtunity"
                );

                if (!upload.IsSuccess)
                    return (400, upload.ErrorMessage);

                fileUrl = upload.FileUrl;
            }

            return await _repository.UpdateSectorOpprtunityAsync(
                sectorOpprtunityId,
                request,
                fileUrl
            );
        }

        // ============================================================
        //                     GET ALL SECTOR OPPORTUNITIES
        // ============================================================
        public async Task<List<NewSectorOpprtunityMasterResponse>> GetSectorOpprtunityAsync(
            bool? isActive,
            int? initiativeId)
        {
            return await _repository.GetSectorOpprtunityAsync(isActive, initiativeId);
        }

        // ============================================================
        //                     GET BY ID
        // ============================================================
        public async Task<NewSectorOpprtunityMasterResponse?> GetSectorOpprtunityByIdAsync(
            int sectorOpprtunityId)
        {
            if (sectorOpprtunityId <= 0)
                return null;

            return await _repository.GetSectorOpprtunityByIdAsync(sectorOpprtunityId);
        }
    }
}
