using System.ComponentModel.DataAnnotations;

namespace MSINS_API.Models.Request
{
    public class NewFaqsRequest
    {
        [Required]
        public int InitiativeId { get; set; }

        [Required]
        [StringLength(255)]
        public string Question { get; set; }

        [Required]
        public string Answer { get; set; }

        [Required]
        public bool IsActive { get; set; }
        [Required]
        public int UserId { get; set; }
    }
}
