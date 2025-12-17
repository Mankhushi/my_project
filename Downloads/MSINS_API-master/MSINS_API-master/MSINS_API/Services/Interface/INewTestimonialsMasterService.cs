using MSINS_API.Models.Request;
using MSINS_API.Models.Response;

namespace MSINS_API.Services.Interface
{
    public interface INewTestimonialsMasterService
    {
        Task<(int Code, string Message)> AddTestimonialAsync(
            NewTestimonialsMasterRequest request
        );

        Task<(int Code, string Message)> UpdateTestimonialAsync(
            int testimonialId,
            NewTestimonialsMasterRequest request
        );

        Task<List<NewTestimonialsMasterResponse>> GetTestimonialsAsync(
            int? initiativeId,
            bool? isActive
        );

        Task<NewTestimonialsMasterResponse?> GetTestimonialByIdAsync(int testimonialId);
    }

}
