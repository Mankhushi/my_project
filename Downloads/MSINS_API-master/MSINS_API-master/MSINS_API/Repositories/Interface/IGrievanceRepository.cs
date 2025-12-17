using MSINS_API.Models.Request;
using MSINS_API.Models.Response;

namespace MSINS_API.Repositories.Interface
{
    public interface IGrievanceRepository
    {
        Task<int> ExecuteGrievanceProcedureAsync(GrievanceRequestModel request);

        Task<PagedResponse<GrievanceResponse>> GetAllGrievance(int pageIndex, int pageSize, string searchTerm, bool isExport);
    }
}
