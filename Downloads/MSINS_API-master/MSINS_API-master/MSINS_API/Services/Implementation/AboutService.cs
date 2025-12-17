using MSINS_API.Models.Request;
using MSINS_API.Models.Response;
using MSINS_API.Repositories.Implementation;
using MSINS_API.Repositories.Interface;
using MSINS_API.Services.Interface;
using SixLabors.ImageSharp;
using System.Net;

namespace MSINS_API.Services.Implementation
{
    public class AboutService : IAboutUsService
    {
        private readonly IFileUploadCustomSizeService _fileUploadService;
        private readonly IAboutUsRepository _aboutUsRepository;

        private readonly string[] _allowedImageFormats = { "image/jpg", "image/jpeg", "image/png", "image/webp" };
        private readonly string[] _allowedPDFFormats = { "application/pdf" };
        private const long _maxImageSizeInKb = 1000;  // 1000 KB
        private const int _requiredWidth = 2035;  // Required width
        private const int _requiredHeight = 2129; // Required height

        public AboutService(IFileUploadCustomSizeService fileUploadService, IAboutUsRepository aboutUsRepository)
        {
            _fileUploadService = fileUploadService;
            _aboutUsRepository = aboutUsRepository;
        }

        public async Task<(int Code, string Message)> AddOrUpdateAboutUsAsync(AboutUsRequest aboutUsDto)
        {
            string? fileUrl = null;
            string? pdfUrl = null;

            // Handle File Upload (if provided)
            //if (aboutUsDto.ImageFile == null)
            //{
            //    return ((int)HttpStatusCode.BadRequest, $"Please select an image to upload.");
            //}
            //else 
            if (aboutUsDto.ImageFile != null)
            {
                // Validate file format
                if (!_allowedImageFormats.Contains(aboutUsDto.ImageFile.ContentType.ToLower()))
                {
                    return ((int)HttpStatusCode.BadRequest, "Invalid file type. Only JPG and PNG are allowed.");
                }

                // Validate file size
                if (aboutUsDto.ImageFile.Length > _maxImageSizeInKb * 1024) // Convert KB to Bytes
                {
                    return ((int)HttpStatusCode.BadRequest, $"File size exceeds {_maxImageSizeInKb} KB limit.");
                }

                // Validate exact image dimensions
                using (var stream = aboutUsDto.ImageFile.OpenReadStream())
                {
                    using (var image = Image.Load(stream))  // Image.Load instead of Image.FromStream
                    {
                        if (image.Width != _requiredWidth || image.Height != _requiredHeight)
                        {
                            return ((int)HttpStatusCode.BadRequest,
                                $"Image must be exactly {_requiredWidth}x{_requiredHeight}px. Uploaded image size: {image.Width}x{image.Height}px.");
                        }
                    }
                }

                var uploadResult = await _fileUploadService.UploadFileAsync(aboutUsDto.ImageFile, "uploads/about");
                if (!uploadResult.IsSuccess)
                {
                    return ((int)HttpStatusCode.BadRequest, uploadResult.ErrorMessage);
                }
                fileUrl = uploadResult.FileUrl;
            }


            // pdf check
            //if (aboutUsDto.PDFFile == null)
            //{
            //    return ((int)HttpStatusCode.BadRequest, $"Please select an PDF to upload.");
            //}
            //else 
            if (aboutUsDto.PDFFile != null)
            {
                // Validate file format
                if (!_allowedPDFFormats.Contains(aboutUsDto.PDFFile.ContentType.ToLower()))
                {
                    return ((int)HttpStatusCode.BadRequest, "Invalid file type. Only PDF allowed.");
                }

                var uploadResult = await _fileUploadService.UploadFileAsync(aboutUsDto.PDFFile, "uploads/about");
                if (!uploadResult.IsSuccess)
                {
                    return ((int)HttpStatusCode.BadRequest, uploadResult.ErrorMessage);
                }
                pdfUrl = uploadResult.FileUrl;
            }

                // Call repository (Stored Procedure Execution)

                var (resultCode, message) = await _aboutUsRepository.AddOrUpdateAboutUsAsync(aboutUsDto, fileUrl, pdfUrl);

            if (resultCode.Equals(1))
            {
                return (200, message);
            }
            else
            {
                return (500, "An unexpected error occurred.");
            }
        }

        public async Task<List<AboutUsResponse>> GetAboutUsAsync()
        {
            return await _aboutUsRepository.GetAboutUsAsync();
        }
    }
}
