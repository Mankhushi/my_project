using System.ComponentModel.DataAnnotations;

namespace MSINS_API.Models.Request
{
    public class InitiativeRequest
    {
        public int? InitiativeId { get; set; } // Nullable for insert

        [Required]
        [StringLength(255)]
        public string InitiativeName { get; set; }

        [Required]
        public string InitiativeDesc { get; set; }

        [Url(ErrorMessage = "Invalid URL format.")]
        public string? InitiativeLink { get; set; }

        public bool? LinkType { get; set; }

        public IFormFile? ImageFile { get; set; } // Nullable to allow optional file uploads in updates

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public int AdminId { get; set; }
    }
}
