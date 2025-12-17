using System.ComponentModel.DataAnnotations;

namespace MSINS_API.Models.Request
{
    public class EcoSystemRequest
    {
        public int? EcoSystemId { get; set; }

        public IFormFile? ImageFile { get; set; }

        [Required]
        [StringLength(255)]
        public string TextOne { get; set; }

        [Required]
        [StringLength(255)]
        public string TextTwo { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public int adminId { get; set; }

    }
}
