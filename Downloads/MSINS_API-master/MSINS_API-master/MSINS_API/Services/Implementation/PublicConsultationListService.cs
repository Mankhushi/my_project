using MSINS_API.Models.Response;
using MSINS_API.Repositories.Interface;
using MSINS_API.Services.Interface;

namespace MSINS_API.Services.Implementation
{
    public class PublicConsultationListService : IPublicConsultationListService
    {
        private readonly IPublicConsultationListRepository _recordRepository;

        public PublicConsultationListService(IPublicConsultationListRepository recordRepository)
        {
            _recordRepository = recordRepository;
        }

        public async Task<PagedResponse<PublicConsultationListResponse>> GetPublicConsultationAll(int pageIndex, int pageSize, string searchTerm, bool isExport)
        {
            return await _recordRepository.GetPublicConsultationAll(pageIndex, pageSize, searchTerm, isExport);
        }
    }
}
