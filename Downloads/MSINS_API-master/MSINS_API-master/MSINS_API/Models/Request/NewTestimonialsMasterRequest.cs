using System.ComponentModel.DataAnnotations;

namespace MSINS_API.Models.Request
{
    public class NewTestimonialsMasterRequest
    {
        [Required(ErrorMessage = "TestimonyGivenBy is required.")]
        [MaxLength(255)]
        public string? TestimonyGivenBy { get; set; }

        [MaxLength(500)]
        public string? TextLineTwon { get; set; }

        public string? Testimony { get; set; }

        public bool? IsActive { get; set; }

        // File URL will be handled separately, but if you're uploading an image, 
        // you can pass the file path or URL here.
        public IFormFile? ProfilePic { get; set; }

        [Required(ErrorMessage = "InitiativeId is required.")]
        public int InitiativeId { get; set; }
        public int AdminId { get; set; }
    }
}
