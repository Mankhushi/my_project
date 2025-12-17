using System.ComponentModel.DataAnnotations;

namespace MSINS_API.Models.Response
{
    public class NewInitiativeMasterResponse
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string HeaderImage { get; set; }
        [Required]
        public string Brief { get; set; }
        [Required]
        public string Schedule { get; set; }
        [Required]
        public string Eligibility { get; set; }
        [Required]
        public string Benefits { get; set; }
        [Required]
        public string Milestone { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public bool IsActive { get; set; }
        [Required]
        public string? ApplyNowLink { get; set; }
        [Required]
        public string? ReachOutEmail { get; set; }


    }
}
