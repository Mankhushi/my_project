namespace MSINS_API.Services.Implementation
{
    public class FileUploadResponse
    {
        public bool IsSuccess { get; set; }
        public string FileUrl { get; set; }
        public string ErrorMessage { get; set; }
    }
}
