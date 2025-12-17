using MSINS_API.Models.Request;
using MSINS_API.Models.Response;
using MSINS_API.Repositories.Interface;
using MSINS_API.Services.Interface;
using SixLabors.ImageSharp;

namespace MSINS_API.Services.Implementation
{
    public class NewIncubatorsMasterService : INewIncubatorsMasterService
    {
        private readonly INewIncubatorsMasterRepository _repository;
        private readonly IFileUploadCustomSizeService _fileUploadService;

        private readonly string[] _allowedImageFormats = { "image/jpg", "image/jpeg", "image/png", "image/webp" };
        private object _NewIncubatorsMasterResponse;
        private const long _maxImageSizeInKb = 200;
        private const int _requiredWidth = 200;
        private const int _requiredHeight = 200;

        public NewIncubatorsMasterService(
            INewIncubatorsMasterRepository repository,
            IFileUploadCustomSizeService fileUploadService)
        {
            _repository = repository;
            _fileUploadService = fileUploadService;
        }

        // ADD
        /// <inheritdoc/>
        public async Task<IncubatorsResultResponse> AddAsync(NewIncubatorsMasterRequest request)
        {
            if (request.Logo == null)
            {
                return new IncubatorsResultResponse
                {
                    IsSuccess = false,
                    Message = "Please upload a logo image.",
                    IncubatorId = 0
                };
            }

            var validate = ValidateAndUploadImage(request.Logo);
            if (!validate.IsSuccess)
            {
                return new IncubatorsResultResponse
                {
                    IsSuccess = false,
                    Message = validate.ErrorMessage!,
                    IncubatorId = 0
                };
            }

            var (success, message, id) = await _repository.AddAsync(request, validate.FileUrl);

            return new IncubatorsResultResponse
            {
                IsSuccess = success,
                Message = message,
                IncubatorId = id
            };
        }

        // UPDATE
        /// <inheritdoc/>
        public async Task<IncubatorsResultResponse> UpdateAsync(NewIncubatorsMasterRequest request)
        {
            string? logoPath = null;

            if (request.Logo != null)
            {
                var validate = ValidateAndUploadImage(request.Logo);
                if (!validate.IsSuccess)
                {
                    return new IncubatorsResultResponse
                    {
                        IsSuccess = false,
                        Message = validate.ErrorMessage!,
                        IncubatorId = 0
                    };
                }

                logoPath = validate.FileUrl;
            }

            var (success, message, id) = await _repository.UpdateAsync(request, logoPath);

            return new IncubatorsResultResponse
            {
                IsSuccess = success,
                Message = message,
                IncubatorId = id
            };
        }

        public Task<NewIncubatorsMasterResponse?> GetByIdAsync(int incubatorId)
        => _repository.GetByIdAsync(incubatorId);

        // WITHOUT PAGINATION + with IsActive
        public async Task<List<NewIncubatorsMasterResponse>> GetAllWithoutPaginationAsync(
            string? citySearch,
            string? sectorSearch,
            string? typeSearch,
            bool? isActive)
        {
            return await _repository.GetAllWithoutPaginationAsync(
                citySearch,
                sectorSearch,
                typeSearch,
                isActive
            );
        }


        //  FINAL FIX: VALIDATE + UPLOAD IMAGE METHOD
        private (bool IsSuccess, string? FileUrl, string? ErrorMessage) ValidateAndUploadImage(IFormFile file)
        {
            if (!_allowedImageFormats.Contains(file.ContentType.ToLower()))
                return (false, null, "Invalid file type. Only JPG, JPEG, PNG, WEBP allowed.");

            if (file.Length > _maxImageSizeInKb * 1024)
                return (false, null, $"File size cannot exceed {_maxImageSizeInKb} KB.");

            using (var stream = file.OpenReadStream())
            using (var image = Image.Load(stream))
            {
                if (image.Width != _requiredWidth || image.Height != _requiredHeight)
                    return (false, null,
                        $"Image must be exactly {_requiredWidth}x{_requiredHeight}px. Uploaded: {image.Width}x{image.Height}px.");
            }

            var upload = _fileUploadService.UploadFileAsync(file, "uploads/incubators").Result;

            if (!upload.IsSuccess)
                return (false, null, upload.ErrorMessage);

            return (true, upload.FileUrl, null);
        }

        Task<global::IncubatorsResultResponse> INewIncubatorsMasterService.AddAsync(NewIncubatorsMasterRequest request)
        {
            throw new NotImplementedException();
        }

        Task<global::IncubatorsResultResponse> INewIncubatorsMasterService.UpdateAsync(NewIncubatorsMasterRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
