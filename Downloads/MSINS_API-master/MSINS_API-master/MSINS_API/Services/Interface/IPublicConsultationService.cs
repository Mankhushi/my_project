using MSINS_API.Models.Request;

namespace MSINS_API.Services.Interface
{
    public interface IPublicConsultationService
    {
        Task<string> SubmitPublicConsultationAsync(PublicationConsultationRequest model);
    }
}
