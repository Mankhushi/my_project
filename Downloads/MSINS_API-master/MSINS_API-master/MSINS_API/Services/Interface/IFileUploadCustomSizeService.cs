using MSINS_API.Services.Implementation;

namespace MSINS_API.Services.Interface
{
    public interface IFileUploadCustomSizeService
    {
        Task<FileUploadResponse> UploadFileAsync(IFormFile file, string folder);
    }
}
