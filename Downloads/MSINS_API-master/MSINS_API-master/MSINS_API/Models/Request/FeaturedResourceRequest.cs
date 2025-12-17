using System.ComponentModel.DataAnnotations;

namespace MSINS_API.Models.Request
{
    public class FeaturedResourceRequest
    {
        public int? FeaturedResourceId { get; set; } = null;

        [Required(ErrorMessage = "Title is required.")]
        [MaxLength(255, ErrorMessage = "Title cannot exceed 255 characters.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Featured Date is required.")]
        public DateTime FeaturedResourceDate { get; set; }
                
        public IFormFile? PDFFile { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public int AdminId { get; set; }
    }
}
