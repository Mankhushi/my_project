using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace MSINS_API.Models.Request
{
    public class LeadershipRequest
    {
        public int? LeadershipId { get; set; } // Nullable for insert/update scenarios

        [Required]
        [MaxLength(255)]
        public string LeaderName { get; set; }

        [Required]
        [MaxLength(255)]
        public string Designation { get; set; }

        public IFormFile? ImageFile { get; set; } // Uploaded Image File

        public bool IsActive { get; set; }

        public int adminId { get; set; }
    }
}
