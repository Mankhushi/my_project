using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace MSINS_API.Models.Request
{
    public class NewContactMasteRequest
    {
        [JsonIgnore]
        public int ContactID { get; set; }

        [Required(ErrorMessage = "Contact Number is required.")]
        public string ContactNumber { get; set; }

        [Required(ErrorMessage = "Email Address is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        public string Address { get; set; }

        public string MapURL { get; set; }   
    }
}
