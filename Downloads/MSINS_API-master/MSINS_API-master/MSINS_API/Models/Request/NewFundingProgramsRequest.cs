using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace MSINS_API.Models.Request
{
    public class NewFundingProgramsRequest
    {
        [Required]
        public IFormFile? Logo { get; set; }

        [Required]
        [StringLength(255)]
        public string FundingAgencyName { get; set; }

        [Required]
        [StringLength(500)]
        public string WebsiteLink { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

        [Required]
        public int AdminId { get; set; }
    }
}
