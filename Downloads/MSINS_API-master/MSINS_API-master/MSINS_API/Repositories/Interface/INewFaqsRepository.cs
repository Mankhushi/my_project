using MSINS_API.Models.Request;
using MSINS_API.Models.Request;
using MSINS_API.Models.Response;

namespace MSINS_API.Repositories.Interface
{
    public interface INewFaqsRepository
    {
        Task<(int Code, string Message)> AddFaqAsync(NewFaqsRequest request);
        Task<(int Code, string Message)> UpdateFaqAsync(int faqsId, NewFaqsRequest request);
        Task<List<NewFaqsResponse>> GetFaqsAsync(int? initiativeId, bool? isActive);

        Task<NewFaqsResponse?> GetFaqByIdAsync(int faqsId);
    }
}
