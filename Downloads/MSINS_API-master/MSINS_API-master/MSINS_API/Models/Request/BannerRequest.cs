using System.ComponentModel.DataAnnotations;

namespace MSINS_API.Models.Request
{
    public class BannerRequest
    {
        public int? BannerId { get; set; } // Nullable, since it's not needed for create

        [Required]
        [StringLength(50)]
        public string BannerType { get; set; }

        [Required]
        [StringLength(255)]
        public string BannerName { get; set; }

        [Url(ErrorMessage = "Invalid URL format.")]
        public string? BannerLink { get; set; }

        public bool? LinkType { get; set; }

        public IFormFile? ImageFile { get; set; } // Nullable to allow optional file uploads in updates

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public int adminId { get; set; }

    }
}
