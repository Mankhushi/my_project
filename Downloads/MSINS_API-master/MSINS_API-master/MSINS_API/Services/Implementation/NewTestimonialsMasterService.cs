using MSINS_API.Models.Request;
using MSINS_API.Models.Response;
using MSINS_API.Repositories.Interface;
using MSINS_API.Services.Interface;
using SixLabors.ImageSharp;

namespace MSINS_API.Services.Implementation
{
    public class NewTestimonialsMasterService : INewTestimonialsMasterService
    {
        private readonly INewTestimonialsMasterRepository _repository;
        private readonly IFileUploadCustomSizeService _fileUploadService;

        private readonly string[] _allowedFormats = { "image/jpg", "image/jpeg", "image/png" };
        private const long _maxSize = 500; // KB
        private const int _width = 154;
        private const int _height = 154;

        public NewTestimonialsMasterService(
            INewTestimonialsMasterRepository repository,
            IFileUploadCustomSizeService fileUploadService)
        {
            _repository = repository;
            _fileUploadService = fileUploadService;
        }

        // ============================================================
        //                        ADD TESTIMONIAL
        // ============================================================
        public async Task<(int Code, string Message)> AddTestimonialAsync(
            NewTestimonialsMasterRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.TestimonyGivenBy))
                return (400, "TestimonyGivenBy is required.");

            if (string.IsNullOrWhiteSpace(request.Testimony))
                return (400, "Testimony is required.");

            if (request.InitiativeId <= 0)
                return (400, "Valid InitiativeId is required.");

            if (request.ProfilePic == null)
                return (400, "Please upload ProfilePic.");

            // Validate file type
            if (!_allowedFormats.Contains(request.ProfilePic.ContentType.ToLower()))
                return (400, "Only JPG and PNG images are allowed.");

            // Validate file size
            if (request.ProfilePic.Length > _maxSize * 1024)
                return (400, $"File size cannot exceed {_maxSize} KB.");

            // Validate image dimensions
            using (var stream = request.ProfilePic.OpenReadStream())
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
                request.ProfilePic,
                "Initiative/Testimonials"
            );

            if (!upload.IsSuccess)
                return (400, upload.ErrorMessage);

            // DB save
            return await _repository.AddTestimonialAsync(
                request,
                upload.FileUrl
            );
        }

        // ============================================================
        //                        UPDATE TESTIMONIAL
        // ============================================================
        public async Task<(int Code, string Message)> UpdateTestimonialAsync(
            int testimonialId,
            NewTestimonialsMasterRequest request)
        {
            if (testimonialId <= 0)
                return (400, "Invalid TestimonialId.");

            if (string.IsNullOrWhiteSpace(request.TestimonyGivenBy))
                return (400, "TestimonyGivenBy is required.");

            if (string.IsNullOrWhiteSpace(request.Testimony))
                return (400, "Testimony is required.");

            if (request.InitiativeId <= 0)
                return (400, "Valid InitiativeId is required.");

            string? fileUrl = null;

            // If image uploaded
            if (request.ProfilePic != null)
            {
                if (!_allowedFormats.Contains(request.ProfilePic.ContentType.ToLower()))
                    return (400, "Only JPG and PNG allowed.");

                if (request.ProfilePic.Length > _maxSize * 1024)
                    return (400, $"File size cannot exceed {_maxSize} KB.");

                using (var stream = request.ProfilePic.OpenReadStream())
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
                    request.ProfilePic,
                    "Initiative/Testimonials"
                );

                if (!upload.IsSuccess)
                    return (400, upload.ErrorMessage);

                fileUrl = upload.FileUrl;
            }

            return await _repository.UpdateTestimonialAsync(
                testimonialId,
                request,
                fileUrl
            );
        }

        // ============================================================
        //                        GET ALL
        // ============================================================
        public async Task<List<NewTestimonialsMasterResponse>> GetTestimonialsAsync(
            int? initiativeId,
            bool? isActive)
        {
            return await _repository.GetTestimonialsAsync(initiativeId, isActive);
        }

        // ============================================================
        //                        GET BY ID
        // ============================================================
        public async Task<NewTestimonialsMasterResponse?> GetTestimonialByIdAsync(
            int testimonialId)
        {
            if (testimonialId <= 0)
                return null;

            return await _repository.GetTestimonialByIdAsync(testimonialId);
        }
    }
}
