using MSINS_API.Models.Request;

namespace MSINS_API.Services.Interface
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string htmlContent, string fromName);
    }
}
