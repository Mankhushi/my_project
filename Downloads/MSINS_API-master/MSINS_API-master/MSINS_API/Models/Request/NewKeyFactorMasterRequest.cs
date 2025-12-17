using Microsoft.AspNetCore.Http;

namespace MSINS_API.Models.Request
{
    public class NewKeyFactorMasterRequest
    {
        public IFormFile? KeyFactorImage { get; set; }   // nvarchar(max) - file upload

        public string? KeyFactorName { get; set; }       // nvarchar(255)

        public bool IsActive { get; set; }

        public int InitiativeId { get; set; }            // FK → Initiative Master
    }
}
