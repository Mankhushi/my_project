using MSINS_API.Models.Request;
using MSINS_API.Models.Response;

namespace MSINS_API.Services.Interface
{
    public interface INewSpeakersMasterService
    {
        Task<(int Code, string Message)> AddSpeakerAsync(NewSpeakersMasterRequest model);
        Task<(int Code, string Message)> UpdateSpeakerAsync(int id, NewSpeakersMasterRequest model);
        Task<(int Code, string Message, NewSpeakerMasterResponse? Data)> GetSpeakerByIdAsync(int id);
        Task<PagedResponse<NewSpeakerMasterResponse>> GetAllSpeakersAsync(int pageNumber, int pageSize, string? searchTerm = null);
    }
}
