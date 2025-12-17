using System.ComponentModel.DataAnnotations;

namespace MSINS_API.Models.Request
{
    public class EventRequest
    {
        public int? EventId { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Event Type cannot exceed 50 characters.")]
        public string EventType { get; set; }

        [Required]
        public DateTime EventDate { get; set; }

        [Required]
        public string EventLocation { get; set; }

        [Required]
        public string EventName { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public string EventDesc { get; set; }

        
        public IFormFile? Image { get; set; } // Accepts file uploads

        [Required]
        public int AdminId { get; set; }
    }
}
