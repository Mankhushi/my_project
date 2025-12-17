using MSINS_API.Services.Interface;

namespace MSINS_API.Services.Implementation
{
    public class FileUploadCustomSizeService : IFileUploadCustomSizeService
    {
        

        public async Task<FileUploadResponse> UploadFileAsync(IFormFile file, string folder)
        {
            var response = new FileUploadResponse();

            try
            {
                
                // Generate a unique file name

                // Get the file extension
                string extension = Path.GetExtension(file.FileName);

                // Get the file name without extension
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file.FileName);

                // Get the current date and time
                string dateTimeStamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");

                // Create the new file name with date and time appended
                string newFileName = $"{fileNameWithoutExtension}_{dateTimeStamp}{extension}";

                var fileName = newFileName;
                var filePath = Path.Combine(folder, fileName);

                // Ensure the uploads directory exists
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                // Save the file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Set response
                response.IsSuccess = true;
                response.FileUrl = $"/{folder}/{fileName}";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMessage = ex.Message;
            }

            return response;
        }
    }
}
