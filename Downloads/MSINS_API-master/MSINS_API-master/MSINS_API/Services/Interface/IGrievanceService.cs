using MSINS_API.Models.Request;
using MSINS_API.Models.Response;

namespace MSINS_API.Services.Interface
{
    public interface IGrievanceService
    {
        Task<(int statusCode, string message)> ProcessGrievanceAsync(GrievanceRequestModel request);

        Task<PagedResponse<GrievanceResponse>> GetAllGrievance(int pageIndex, int pageSize, string searchTerm, bool isExport);
    }
}
