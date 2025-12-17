using System.ComponentModel.DataAnnotations;

namespace MSINS_API.Models.Request
{
    public class PartnersRequest
    {
        public int? PartnerId { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "Partner name cannot exceed 255 characters.")]
        public string PartnerName { get; set; }

        [Url(ErrorMessage = "Invalid URL format.")]
        public string? PartnerLink { get; set; }

        public bool? LinkType { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public int adminId { get; set; }

        public IFormFile? ImageFile { get; set; } // Nullable to allow optional file uploads in updates

    }
}
