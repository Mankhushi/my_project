using MSINS_API.Models.Request;
using MSINS_API.Repositories.Interface;
using MSINS_API.Services.Interface;

namespace MSINS_API.Services.Implementation
{
    public class PublicConsultationService : IPublicConsultationService
    {
        private readonly IPublicConsultationRepository _repository;
        private readonly IEmailService _emailService;

        public PublicConsultationService(IPublicConsultationRepository repository, IEmailService emailService)
        {
            _repository = repository;
            _emailService = emailService;
        }

        public async Task<string> SubmitPublicConsultationAsync(PublicationConsultationRequest model)
        {

            int rowsAffected = await _repository.SavePublicConsultationAsync(model);
            if(rowsAffected.Equals(-5))
            {
                return "File upload failed";
            }
            else if (rowsAffected > 0)
            {
                // Send acknowledgment email
                string subject = "Acknowledgment of Your Submission";
                string htmlContent = $"<p>Dear {model.FullName},</p><p>Thank you for your valuable contribution.</p>";
                await _emailService.SendEmailAsync(model.Email, subject, htmlContent, "Maharashtra State Innovation Society");

                return "Submission successful.";
            }

            return "Failed to save the submission.";

        }
    }
}
