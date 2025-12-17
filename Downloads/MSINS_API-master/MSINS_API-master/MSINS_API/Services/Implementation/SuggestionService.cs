using MSINS_API.Models.Request;
using MSINS_API.Models.Response;
using MSINS_API.Repositories.Implementation;
using MSINS_API.Repositories.Interface;
using MSINS_API.Services.Interface;

namespace MSINS_API.Services.Implementation
{
    public class SuggestionService : ISuggestionService
    {
        private readonly ISuggestionRepository _suggestionRepository;

        public SuggestionService(ISuggestionRepository suggestionRepository)
        {
            _suggestionRepository = suggestionRepository;
        }

        public async Task<PagedResponse<SuggestionResponse>> GetAllSuggestion(int pageIndex, int pageSize, string searchTerm, bool isExport)
        {
            return await _suggestionRepository.GetAllSuggestion(pageIndex, pageSize, searchTerm, isExport);
        }

        public async Task<(int statusCode, string message)> ProcessSuggestionAsync(SuggestionRequestModel request)
        {
            // Call the repository to process the suggestion via a stored procedure
            var repositoryResult = await _suggestionRepository.ExecuteSuggestionProcedureAsync(request);

            if (repositoryResult == 0) // Assuming 0 means success
            {
                return (200, "Thank you for your suggestion! Your response has been successfully submitted.");
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
