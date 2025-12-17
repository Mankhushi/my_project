using System.ComponentModel.DataAnnotations;

namespace MSINS_API.Models.Request
{
    public class PartnersQueryParamsRequest
    {
        public string? PartnerName { get; set; }

        public bool? IsActive { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "PageIndex must be greater than 0.")]
        public int PageIndex { get; set; } = 1;

        [Range(1, 100, ErrorMessage = "PageSize must be between 1 and 100.")]
        public int PageSize { get; set; } = 10;
    }
}
