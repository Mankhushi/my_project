using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;


namespace MSINS_API.Models.Request
{
    public class PublicationConsultationRequest
    {
        /// <summary>
        /// This field is required and cannot exceed 255 characters.<br />
        /// Example : John Robert D'souza
        /// </summary>
        [Required(ErrorMessage = "Full Name is required.")]
        [StringLength(255, ErrorMessage = "Full Name cannot exceed 255 characters.")]
        public string FullName { get; set; }

        public string Email { get; set; }
        public string? ContactNumber { get; set; }
        public string? CityDistrict { get; set; }
        public string? OrganizationName { get; set; }
        public string? ExpertiseSector { get; set; }
        public string? ExpertiseSectorOther { get; set; }
        public string? AspectPolicy { get; set; }
        public string? Suggestion { get; set; }
        public string? Rating { get; set; }
        public string? RecommendateProgram { get; set; }
        public IFormFile? file1 { get; set; }
    }
}
