using System.ComponentModel.DataAnnotations;

namespace MSINS_API.Models.Request
{
    public class TenderNotificationRequest
    {
        public int? TenderNotificationId { get; set; }

        [MaxLength(255)]
        [Required(ErrorMessage = "Tender Id is required.")]
        public string TenderId { get; set; }

        [MaxLength(255)]
        [Required(ErrorMessage = "Reference No. is required.")]
        public string RefNo { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [MaxLength(255)]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Category is required.")]
        [MaxLength(255)]
        public string Category { get; set; }

        [Required(ErrorMessage = "Published Date is required.")]
        public DateTime PublishedDate { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        [MaxLength(255)]
        public string Status { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public int AdminId { get; set; }
    }
}
