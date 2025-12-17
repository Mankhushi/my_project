using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace MSINS_API.Models.Request
{
    public class NewSectorOpprtunityMasterRequest
    {
        public IFormFile? SectorOpprtunityImage { get; set; }  // nvarchar(max) → file upload

        [Required(ErrorMessage = "SectorOpprtunityName is required.")]
        public string? SectorOpprtunityName { get; set; }      // nvarchar(max)

        public bool IsActive { get; set; } = true;             // default true

        [Required(ErrorMessage = "InitiativeId is required.")]
        public int InitiativeId { get; set; }                  // FK to InitiativeMaster

        public int AdminId { get; set; }
    }
}
