using System.ComponentModel.DataAnnotations;

namespace MSINS_API.Models.Request
{
    public class ExecutiveRequest
    {
        public int? ExecutiveId { get; set; } // Nullable for creation

        public IFormFile? ImageFile { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        [StringLength(255)]
        public string ExecutiveName { get; set; }

        public string? ExecutiveDesc { get; set; }

        [Required]
        public int AdminId { get; set; }
    }
}
