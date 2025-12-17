using System.ComponentModel.DataAnnotations;

namespace MSINS_API.Models.Request
{
    public class FeaturedResourceQueryParamsRequest
    {
        public bool? IsActive { get; set; } // Optional filter

        public DateTime? StartDate { get; set; } // Optional start date filter

        public DateTime? EndDate { get; set; } // Optional end date filter

        public string? Title { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "PageIndex must be greater than 0.")]
        public int PageIndex { get; set; } = 1;


        [Range(1, 10, ErrorMessage = "PageSize must be between 1 and 10.")]
        public int PageSize { get; set; } = 10;
    }
}
