using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace MSINS_API.Models.Request
{
    public class NewTypeMasterRequest
    {
        //[SwaggerSchema(ReadOnly = true, Description = "Auto-generated Type ID")]
        //public int TypeId { get; set; }

        [Required]
        [SwaggerSchema("Enter Type Name")]
        public string TypeName { get; set; } = string.Empty;

        [Required]
        [SwaggerSchema("Select Sector from dropdown", Format = "sector-dropdown")]
        public int SectorId { get; set; }

        [Required]
        [SwaggerSchema("Set status: true for Active, false for Inactive")]
        public bool IsActive { get; set; }
        [Required]
        public int AdminId { get; set; }
        public int TypeId { get; internal set; }
    }
}
