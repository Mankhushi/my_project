using System.ComponentModel.DataAnnotations;

namespace MSINS_API.Models.Request
{
    public class EcoSystemQueryParamsRequest
    {
        public string? TextOne { get; set; }


        public string? TextTwo { get; set; }


        public bool? IsActive { get; set; }


        [Range(1, int.MaxValue, ErrorMessage = "PageIndex must be greater than 0.")]
        public int PageIndex { get; set; } = 1;



        [Range(1, 20, ErrorMessage = "PageSize must be between 1 and 20.")]
        public int PageSize { get; set; } = 10;
    }
}
