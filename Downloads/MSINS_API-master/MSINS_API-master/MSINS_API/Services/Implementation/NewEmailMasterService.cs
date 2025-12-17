using MSINS_API.Models.Request;
using MSINS_API.Models.Response;
using MSINS_API.Repositories.Interface;
using MSINS_API.Services.Interface;
using System.Net;

namespace MSINS_API.Services.Implementation
{
    public class NewEmailMasterService : INewEmailMasterService
    {
        private readonly INewEmailMasterRepository _emailRepository;

        public NewEmailMasterService(INewEmailMasterRepository emailRepository)
        {
            _emailRepository = emailRepository;
        }

        // -------------------------------------------------------------
        // ADD EMAIL
        // -------------------------------------------------------------
        public async Task<(int Code, string Message, int EmailId)> AddEmailAsync(NewEmailMasterRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email))
            {
                return ((int)HttpStatusCode.BadRequest, "Email is required.", 0);
            }

            var (resultCode, message, emailId) = await _emailRepository.AddEmailAsync(request);

            if (resultCode == 1)
                return (200, "Email added successfully.", emailId);

            return (500, message ?? "An unexpected error occurred.", 0);
        }

        //------------------------- UPDATE EMAIL -----------------------------
        public async Task<(int Code, string Message)> UpdateEmailAsync(int id, NewEmailMasterRequest request)
        {
            if (id <= 0)
            {
                return ((int)HttpStatusCode.BadRequest, "Invalid Email ID.");
            }

            if (string.IsNullOrWhiteSpace(request.Email))
            {
                return ((int)HttpStatusCode.BadRequest, "Email is required.");
            }

            var (resultCode, message) = await _emailRepository.UpdateEmailAsync(id, request);

            if (resultCode == 1)
                return (200, "Email updated successfully.");

            return (500, message ?? "An unexpected error occurred.");
        }

        //-------------------- GET EMAIL BY ID -----------------------------
        public async Task<NewEmailMasterResponse?> GetEmailByIdAsync(int id)
        {
            return await _emailRepository.GetEmailByIdAsync(id);
        }

        // ---------------------- GET ALL (QueryParams) ----------------------
        public async Task<PagedResponse<NewEmailMasterResponse>> GetAllEmailsAsync(
            NewEmailMasterQueryParams queryParams)
        {
            return await _emailRepository.GetAllEmailsAsync(queryParams);
        }

        // ---------------------- GET ALL (Raw parameters) -------------------
        public async Task<PagedResponse<NewEmailMasterResponse>> GetAllEmailsAsync(
            int pageNumber,
            int pageSize,
            string? searchTerm,
            bool? isActive)
        {
            var queryParams = new NewEmailMasterQueryParams
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                SearchTerm = searchTerm,
                IsActive = isActive
            };

            return await _emailRepository.GetAllEmailsAsync(queryParams);
        }

        // ---------------------- EXPORT EMAILS ------------------------------
        public async Task<List<NewEmailMasterResponse>> ExportEmailsAsync(bool? isActive)
        {
            return await _emailRepository.ExportEmailsAsync(isActive);
        }
    }
}
