using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace MSINS_API.Models.Request
{
    public class NewPhotographsMasterRequest
    {
        [Required(ErrorMessage = "PhotographName is required.")]
        public string? PhotographName { get; set; }

        // File upload from frontend
        public IFormFile? PhotographFile { get; set; }

        public bool? IsActive { get; set; }

        [Required(ErrorMessage = "InitiativeId is required.")]
        public int InitiativeId { get; set; }
        public int AdminId { get; set; }

    }
}
