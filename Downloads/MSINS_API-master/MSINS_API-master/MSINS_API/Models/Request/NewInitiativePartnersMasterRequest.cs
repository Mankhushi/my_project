using Microsoft.AspNetCore.Http;

namespace MSINS_API.Models.Request
{
    public class NewInitiativePartnersMasterRequest
    {
        public string? PartnerType { get; set; }

        // File upload for partner image
        public IFormFile? PartnerImage { get; set; }

        public bool IsActive { get; set; }

        public int InitiativeId { get; set; }
        public string? AdminId { get; set; }

    }
}
