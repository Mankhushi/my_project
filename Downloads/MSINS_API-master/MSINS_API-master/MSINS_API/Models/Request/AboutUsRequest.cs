using System.ComponentModel.DataAnnotations;

namespace MSINS_API.Models.Request
{
    public class AboutUsRequest
    {
        [Required]
        public string TextOne { get; set; }

        [Required]
        public string TextTwo { get; set; }

        public IFormFile? ImageFile { get; set; } // Nullable to allow optional file uploads in updates

        public IFormFile? PDFFile { get; set; } // Nullable to allow optional file uploads in updates

        [Required]
        public int adminId { get; set; }
    }
}
