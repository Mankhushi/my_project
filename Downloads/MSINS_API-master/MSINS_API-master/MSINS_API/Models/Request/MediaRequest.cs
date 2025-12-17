using System.ComponentModel.DataAnnotations;

namespace MSINS_API.Models.Request
{
    public class MediaRequest
    {
        public int? MediaId { get; set; } // Nullable for add/update scenario

        [Required(ErrorMessage = "Media date is required.")]
        public DateTime MediaDate { get; set; }

        public IFormFile? Image { get; set; }

        [Required(ErrorMessage = "IsActive Status is required.")]
        public bool IsActive { get; set; }

        [Required(ErrorMessage = "Media description is required.")]
        public string MediaDesc { get; set; } = string.Empty;

        [Required(ErrorMessage = "Media name is required.")]
        [MaxLength(255, ErrorMessage = "Media name cannot exceed 255 characters.")]
        public string MediaName { get; set; } = string.Empty;

        [Required]
        [Url(ErrorMessage = "Invalid URL format for MediaLink.")]
        public string? MediaLink { get; set; }
        [Required]
        public int AdminId { get; set; }
    }
}
