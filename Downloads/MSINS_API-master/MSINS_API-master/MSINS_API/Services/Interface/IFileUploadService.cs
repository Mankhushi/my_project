using MSINS_API.Models.Response;

namespace MSINS_API.Services.Implementation
{
    public interface IFileUploadService
    {
        Task<FileUploadResponse> UploadFileAsync(IFormFile file,string folder);
    }
}
