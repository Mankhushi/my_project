using System.ComponentModel.DataAnnotations;

namespace MSINS_API.Models.Request
{
    public class LinkedInRequest
    {
        public int? LinkedInId { get; set; }

        [Required]
        [Url(ErrorMessage = "Invalid URL format.")]
        public string Link { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public int AdminId { get; set; }
    }
}
