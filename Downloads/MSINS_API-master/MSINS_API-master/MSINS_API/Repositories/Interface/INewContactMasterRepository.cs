using MSINS_API.Models.Request;
using MSINS_API.Models.Response;

namespace MSINS_API.Repositories.Interface
{
    public interface INewContactMasterRepository
    {
        Task<(int Code, string Message)> UpdateContactAsync(NewContactMasteRequest model);
        Task<NewContactMasterResponse?> GetContactByIdAsync(int contactId);
    }
}
