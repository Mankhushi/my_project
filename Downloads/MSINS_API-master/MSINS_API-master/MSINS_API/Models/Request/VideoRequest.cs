using System.ComponentModel.DataAnnotations;

namespace MSINS_API.Models.Request
{
    public class VideoRequest
    {
        public int? VideoId { get; set; } // Nullable for create/update flexibility

        [Required(ErrorMessage = "Link is required.")]
        [Url(ErrorMessage = "Invalid URL format.")]
        public string Link { get; set; } = string.Empty;

        [Required(ErrorMessage = "Title is required.")]
        [MaxLength(255, ErrorMessage = "Title cannot exceed 255 characters.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "IsFeatured status is required.")]
        public bool IsFeatured { get; set; }

        [Required(ErrorMessage = "IsLatest status is required.")]
        public bool IsLatest { get; set; }

        [Required(ErrorMessage = "IsActive status is required.")]
        public bool IsActive { get; set; }

        [Required]
        public int AdminId { get; set; }
    }
}
