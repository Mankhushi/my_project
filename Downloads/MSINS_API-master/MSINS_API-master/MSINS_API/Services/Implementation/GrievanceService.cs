using MSINS_API.Models.Request;
using MSINS_API.Models.Response;
using MSINS_API.Repositories.Implementation;
using MSINS_API.Repositories.Interface;
using MSINS_API.Services.Interface;

namespace MSINS_API.Services.Implementation
{
    public class GrievanceService: IGrievanceService
    {
        private readonly IGrievanceRepository _grievanceRepository;

        public GrievanceService(IGrievanceRepository grievanceRepository)
        {
            _grievanceRepository = grievanceRepository;
        }

        public async Task<PagedResponse<GrievanceResponse>> GetAllGrievance(int pageIndex, int pageSize, string searchTerm, bool isExport)
        {
            return await _grievanceRepository.GetAllGrievance(pageIndex, pageSize, searchTerm, isExport);
        }

        public async Task<(int statusCode, string message)> ProcessGrievanceAsync(GrievanceRequestModel request)
        {
            // Call the repository to process the suggestion via a stored procedure
            var repositoryResult = await _grievanceRepository.ExecuteGrievanceProcedureAsync(request);

            if (repositoryResult == 0) // Assuming 0 means success
            {
                return (200, "Thank you for submitting your grievance. Your request has been successfully received and will be reviewed promptly.");
            }
            else if (repositoryResult == -1) // Example of a SQL-specific status
            {
                return (400, "Duplicate entry or invalid data.");
            }
            else
            {
                return (500, "An unexpected error occurred while processing the suggestion.");
            }
        }
    }
}
