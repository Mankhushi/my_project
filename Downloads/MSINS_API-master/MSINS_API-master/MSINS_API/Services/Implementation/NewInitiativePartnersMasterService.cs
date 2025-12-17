using MSINS_API.Models.Request;
using MSINS_API.Models.Response;
using MSINS_API.Repositories.Interface;
using MSINS_API.Services.Interface;
using SixLabors.ImageSharp;

namespace MSINS_API.Services.Implementation
{
    public class NewInitiativePartnersMasterService : INewInitiativePartnersMasterService
    {
        private readonly INewInitiativePartnersMasterRepository _repository;
        private readonly IFileUploadCustomSizeService _fileUploadService;

        private readonly string[] _allowedImageFormats = { "image/jpg", "image/jpeg", "image/png" };
        private const long _maxImageSizeKb = 500;
        private const int _width = 154;
        private const int _height = 154;

        public NewInitiativePartnersMasterService(
            INewInitiativePartnersMasterRepository repository,
            IFileUploadCustomSizeService fileUploadService)
        {
            _repository = repository;
            _fileUploadService = fileUploadService;
        }

        // ADD
        public async Task<(int Code, string Message)> AddPartnerAsync(
            NewInitiativePartnersMasterRequest request,
            string? fileUrl)
        {
            if (request.PartnerImage == null)
                return (400, "Please upload partner image.");

            if (!_allowedImageFormats.Contains(request.PartnerImage.ContentType.ToLower()))
                return (400, "Only JPG/PNG allowed");

            if (request.PartnerImage.Length > _maxImageSizeKb * 1024)
                return (400, $"Max file size is {_maxImageSizeKb} KB");

            using (var imgStream = request.PartnerImage.OpenReadStream())
            using (var image = Image.Load(imgStream))
            {
                if (image.Width != _width || image.Height != _height)
                    return (400, $"Image must be exactly {_width}x{_height} px");
            }

            var upload = await _fileUploadService.UploadFileAsync(
                request.PartnerImage,
                "Initiative/InitiativePartners"
            );

            if (!upload.IsSuccess)
                return (400, upload.ErrorMessage);

            fileUrl = upload.FileUrl;

            return await _repository.AddPartnerAsync(request, fileUrl);
        }

        // UPDATE
        public async Task<(int Code, string Message)> UpdatePartnerAsync(
            int partnerId,
            NewInitiativePartnersMasterRequest request,
            string? fileUrl)
        {
            if (request.PartnerImage != null)
            {
                if (!_allowedImageFormats.Contains(request.PartnerImage.ContentType.ToLower()))
                    return (400, "Only JPG/PNG allowed");

                if (request.PartnerImage.Length > _maxImageSizeKb * 1024)
                    return (400, $"Max file size is {_maxImageSizeKb} KB");

                using (var imgStream = request.PartnerImage.OpenReadStream())
                using (var image = Image.Load(imgStream))
                {
                    if (image.Width != _width || image.Height != _height)
                        return (400, $"Image must be exactly {_width}x{_height} px");
                }

                var upload = await _fileUploadService.UploadFileAsync(
                     request.PartnerImage,
                     "Initiative/InitiativePartners"
                 );

                if (!upload.IsSuccess)
                    return (400, upload.ErrorMessage);

                fileUrl = upload.FileUrl;
            }

            return await _repository.UpdatePartnerAsync(partnerId, request, fileUrl);
        }

        public async Task<List<NewInitiativePartnersMasterResponse>> GetPartnersAsync(int? initiativeId, bool? isActive)
            => await _repository.GetPartnersAsync(initiativeId, isActive);

        public async Task<NewInitiativePartnersMasterResponse?> GetPartnerByIdAsync(int partnerId)
            => await _repository.GetPartnerByIdAsync(partnerId);
    }
}
