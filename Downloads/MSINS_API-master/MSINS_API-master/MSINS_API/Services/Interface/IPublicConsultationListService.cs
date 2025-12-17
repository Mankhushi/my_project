using MSINS_API.Models.Response;

namespace MSINS_API.Services.Interface
{
    public interface IPublicConsultationListService
    {
        Task<PagedResponse<PublicConsultationListResponse>> GetPublicConsultationAll(int pageIndex, int pageSize, string searchTerm, bool isExport);
    }
}
