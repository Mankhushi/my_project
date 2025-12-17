using System.ComponentModel.DataAnnotations;

namespace MSINS_API.Models.Request
{
    public class NewCityMasterRequest
    {
        [Required(ErrorMessage = "City Name is required.")]
        public string CityName { get; set; }
        [Required(ErrorMessage = "IsActive status is required.")]
        public bool IsActive { get; set; }

        [Required(ErrorMessage = "UserId is required.")]
        public int adminId { get; set; }
    }
}
