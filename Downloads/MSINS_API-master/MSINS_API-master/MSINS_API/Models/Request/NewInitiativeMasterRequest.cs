using Microsoft.AspNetCore.Http;

namespace MSINS_API.Models.Request
{
    public class NewInitiativeMasterRequest
    {
        public string? Title { get; set; }
        public string? Brief { get; set; }
        public string? Schedule { get; set; }
        public string? Eligibility { get; set; }
        public string? Benefits { get; set; }
        public string? Milestone { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public IFormFile? HeaderImageFile { get; set; }
        public string? ApplyNowLink { get; set; }
        public string? ReachOutEmail { get; set; }
        public string? AdminId { get; internal set; }
    }
}
