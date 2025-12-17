using MSINS_API.Models.Response;

namespace MSINS_API.Repositories.Interface
{
    public interface IPublicConsultationListRepository
    {
        Task<PagedResponse<PublicConsultationListResponse>> GetPublicConsultationAll(int pageIndex, int pageSize, string searchTerm, bool isExport);
    }
}
