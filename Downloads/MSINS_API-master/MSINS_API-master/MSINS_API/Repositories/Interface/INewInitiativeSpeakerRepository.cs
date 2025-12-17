using MSINS_API.Models.Request;
using MSINS_API.Models.Response;

public interface INewInitiativeSpeakerRepository
{
    Task<(int Code, string Message)> AddSpeakerAsync(NewInitiativeSpeakerRequest request, string fileUrl);
    Task<(int Code, string Message)> UpdateSpeakerAsync(int speakerId, NewInitiativeSpeakerRequest request, string fileUrl);
    Task<NewInitiativeSpeakerResponse?> GetSpeakerByIdAsync(int speakerId);
    Task<PagedResponse<NewInitiativeSpeakerResponse>> GetAllSpeakersAsync(string? name, string? designation, bool? isActive, int pageIndex, int pageSize);
}



