using System.ComponentModel.DataAnnotations;

namespace MSINS_API.Models.Request
{
    public class NewSpeakersMasterRequest
    {
        internal object adminId;

        // Use nullable types for optional fields
        [Required]
        [StringLength(255)]
        public string SpeakersName { get; set; }

        [Required]
        [StringLength(255)]
        public string Designation { get; set; }

        [Required]
        [StringLength(255)]
        public string Organization { get; set; }
        public IFormFile? ProfilePic { get; set; }  // optional image
        public bool IsActive { get; set; } = true;        // optional for updates

        [Required]
        //public int adminId { get; set; }
        public int InitiativeId { get; set; }
    }
}
