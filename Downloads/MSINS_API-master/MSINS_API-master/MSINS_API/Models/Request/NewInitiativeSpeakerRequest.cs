using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace MSINS_API.Models.Request
{
    public class NewInitiativeSpeakerRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Designation { get; set; }

        [Required]
        public bool IsActive { get; set; }

        // ✔ HERE IS YOUR NEW PROFILE PIC
        [Required(ErrorMessage = "Please upload a profile picture.")]
        public IFormFile ProfilePicUrl { get; set; }
       

    }
}
