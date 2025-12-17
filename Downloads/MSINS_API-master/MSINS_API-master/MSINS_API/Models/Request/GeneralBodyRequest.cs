using System.ComponentModel.DataAnnotations;

namespace MSINS_API.Models.Request
{
    public class GeneralBodyRequest
    {
        public int? GeneralBodyId { get; set; }  // Primary Key

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [Required]
        [StringLength(255)]
        public string Designation { get; set; }

        [Required]
        [StringLength(255)]
        public string Position { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public int adminId { get; set; }
    }
}
