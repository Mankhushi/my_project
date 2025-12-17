using MSINS_API.Models.Request;
using MSINS_API.Models.Response;
using MSINS_API.Repositories.Interface;
using MSINS_API.Services.Interface;
using System.Net;

namespace MSINS_API.Services.Implementation
{
    public class NewContactMasterService : INewContactMasterService
    {
        private readonly INewContactMasterRepository _contactRepository;

        public NewContactMasterService(INewContactMasterRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }


        // -----------------UPDATE CONTACT-----------------------------------------------------
        public async Task<(int Code, string Message)> UpdateContactAsync(NewContactMasteRequest model)
        {
            if (model.ContactID <= 0)
                return ((int)HttpStatusCode.BadRequest, "ContactID is required for update.");

            var (resultCode, message) = await _contactRepository.UpdateContactAsync(model);

            if (resultCode == 1)
                return (200, message);

            return ((int)HttpStatusCode.InternalServerError, "An unexpected error occurred.");
        }

        //---------------------------GET CONTACT BY ID -------------------------------------
        
        public async Task<NewContactMasterResponse?> GetContactByIdAsync(int contactId)
        {
            return await _contactRepository.GetContactByIdAsync(contactId);
        }
    }
}
