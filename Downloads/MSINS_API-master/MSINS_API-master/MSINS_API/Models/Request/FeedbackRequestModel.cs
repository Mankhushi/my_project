using System.ComponentModel.DataAnnotations;

namespace MSINS_API.Models.Request
{
    public class FeedbackRequestModel
    {/// <summary>
    /// test
    /// </summary>
        [Required]
        [StringLength(255, MinimumLength = 3)]
        public string fullName { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 3)]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string email { get; set; }

        [Required]
        public string FeedbackType { get; set; }

        [Required]
        public string countryCode { get; set; }

        [Required]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Mobile number must be exactly 10 digits.")]
        public string mobile { get; set; }

        [Required]
        public string subject { get; set; }

        [Required]
        public string description { get; set; }
    }
}
