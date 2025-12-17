using MSINS_API.Models.Request;
using MSINS_API.Models.Response;

namespace MSINS_API.Repositories.Interface
{
    public interface INewSpeakersMasterRepository
    {
        Task<(int Code, string Message)> AddSpeakerAsync(NewSpeakersMasterRequest model, string? fileUrl);
        Task<(int Code, string Message)> UpdateSpeakerAsync(int id, NewSpeakersMasterRequest model, string? fileUrl);
        Task<NewSpeakerMasterResponse?> GetSpeakerByIdAsync(int id);
        Task<PagedResponse<NewSpeakerMasterResponse>> GetAllSpeakersAsync(int pageNumber, int pageSize, string? searchTerm = null);
    }
}
