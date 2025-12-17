using MSINS_API.Models.Request;
using MSINS_API.Models.Request;
using MSINS_API.Models.Response;
using MSINS_API.Repositories.Interface;
using MSINS_API.Services.Interface;
using System.Net;

namespace MSINS_API.Services.Implementation
{
    public class NewFaqsService : INewFaqsService
    {
        private readonly INewFaqsRepository _faqsRepository;

        public NewFaqsService(INewFaqsRepository faqsRepository)
        {
            _faqsRepository = faqsRepository;
        }

        // ================================
        //           ADD FAQ
        // ================================
        public async Task<(int Code, string Message)> AddFaqAsync(NewFaqsRequest request)
        {
            // Basic Validation
            if (string.IsNullOrWhiteSpace(request.Question))
                return ((int)HttpStatusCode.BadRequest, "Question is required.");

            if (string.IsNullOrWhiteSpace(request.Answer))
                return ((int)HttpStatusCode.BadRequest, "Answer is required.");

            var (resultCode, message) = await _faqsRepository.AddFaqAsync(request);

            if (resultCode == 1)
                return (200, message);
            else
                return (500, "Failed to add FAQ. Please try again.");
        }

        // ================================
        //           UPDATE FAQ
        // ================================
        public async Task<(int Code, string Message)> UpdateFaqAsync(int faqsId, NewFaqsRequest request)
        {
            if (faqsId <= 0)
                return ((int)HttpStatusCode.BadRequest, "Invalid FAQ ID.");

            if (string.IsNullOrWhiteSpace(request.Question))
                return ((int)HttpStatusCode.BadRequest, "Question is required.");

            if (string.IsNullOrWhiteSpace(request.Answer))
                return ((int)HttpStatusCode.BadRequest, "Answer is required.");

            var (resultCode, message) = await _faqsRepository.UpdateFaqAsync(faqsId, request);

            if (resultCode == 1)
                return (200, message);
            else if (resultCode == 0)
                return (404, "FAQ not found.");
            else
                return (500, "Failed to update FAQ. Something went wrong.");
        }

        // ================================
        //      GET ALL FAQs (List)
        // ================================
        public async Task<List<NewFaqsResponse>> GetFaqsAsync(int? initiativeId, bool? isActive)
        {
            return await _faqsRepository.GetFaqsAsync(initiativeId, isActive);
        }



        // ================================
        //         GET FAQ BY ID
        // ================================
        public async Task<NewFaqsResponse?> GetFaqByIdAsync(int faqsId)
        {
            if (faqsId <= 0)
                return null;

            return await _faqsRepository.GetFaqByIdAsync(faqsId);
        }
    }
}
