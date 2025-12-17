using System.ComponentModel.DataAnnotations;

namespace MSINS_API.Models.Request
{
    public class GovernmentResolutionQueryParamsRequest
    {
        public bool? IsActive { get; set; } // Optional filter

        public string? Title { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "PageIndex must be greater than 0.")]
        public int PageIndex { get; set; } = 1;


        [Range(1, 10, ErrorMessage = "PageSize must be between 1 and 10.")]
        public int PageSize { get; set; } = 10;
    }
}
