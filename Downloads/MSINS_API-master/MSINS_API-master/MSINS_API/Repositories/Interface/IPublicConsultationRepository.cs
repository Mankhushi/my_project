using MSINS_API.Models.Request;

namespace MSINS_API.Repositories.Interface
{
    public interface IPublicConsultationRepository
    {
        Task<int> SavePublicConsultationAsync(PublicationConsultationRequest model);
    }
}
