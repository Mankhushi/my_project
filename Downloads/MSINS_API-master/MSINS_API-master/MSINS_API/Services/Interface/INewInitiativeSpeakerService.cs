using MSINS_API.Models.Request;
using MSINS_API.Models.Response;

namespace MSINS_API.Services.Interface
{
    public interface INewInitiativeSpeakerService
    {
        // Add new speaker with profile pic upload handled inside service
        Task<(int Code, string Message)> AddSpeakerAsync(NewInitiativeSpeakerRequest request);

        // Update existing speaker (profile pic optional)
        Task<(int Code, string Message)> UpdateSpeakerAsync(int speakerId, NewInitiativeSpeakerRequest request);

        // Get single speaker by Id
        Task<NewInitiativeSpeakerResponse?> GetSpeakerByIdAsync(int speakerId);


        // Listing + pagination + filters
        Task<PagedResponse<NewInitiativeSpeakerResponse>> GetAllSpeakersAsync(string? name, string? designation, bool? isActive, int pageIndex, int pageSize);
        //Task GetAllSpeakersWithoutPaginationAsync(string? name, string? designation, bool? isActive);
    }
}
