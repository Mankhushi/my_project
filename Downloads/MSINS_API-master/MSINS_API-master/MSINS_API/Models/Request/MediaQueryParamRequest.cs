using System.ComponentModel.DataAnnotations;

namespace MSINS_API.Models.Request
{
    public class MediaQueryParamRequest
    {
        [MaxLength(255)]
        public string? Search { get; set; }

        public bool? IsActive { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "PageIndex must be greater than 0.")]
        public int PageIndex { get; set; } = 1;

        [Range(1, 20, ErrorMessage = "PageSize must be between 1 and 20.")]
        public int PageSize { get; set; } = 10;

        public DateTime? MediaStartDate { get; set; }

        public DateTime? MediaEndDate { get; set; }

        
    }
}
