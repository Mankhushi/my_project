using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MSINS_API.Models.Request
{
    public class NewEmailMasterRequest
    {
        [JsonIgnore]  
        public int? Id { get; set; }


        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email format.")]
        [MaxLength(150, ErrorMessage = "Email cannot exceed 150 characters.")]
        public string Email { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;   

        public int? adminId { get; set; }
    }
}
