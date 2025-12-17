using MSINS_API.Models.Request;
using MSINS_API.Models.Response;

namespace MSINS_API.Repositories.Interface
{
    public interface INewTestimonialsMasterRepository
    {
        // ADD
        Task<(int Code, string Message)> AddTestimonialAsync(
            NewTestimonialsMasterRequest request,
            string? fileUrl
        );

        // UPDATE
        Task<(int Code, string Message)> UpdateTestimonialAsync(
            int testimonialId,
            NewTestimonialsMasterRequest request,
            string? fileUrl
        );

        // GET ALL
        Task<List<NewTestimonialsMasterResponse>> GetTestimonialsAsync(
            int? initiativeId,
            bool? isActive
        );

        // GET BY ID
        Task<NewTestimonialsMasterResponse?> GetTestimonialByIdAsync(int testimonialId);
    }
}
