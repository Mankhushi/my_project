using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class NewIncubatorsMasterRequest
{
    [JsonIgnore]
    [SwaggerSchema(ReadOnly = true, WriteOnly = true)]
    public int? IncubatorId { get; set; }

    [Required]
    public string IncubatorName { get; set; } = string.Empty;

    [Required]
    public int CityId { get; set; }

    [Required]
    public int SectorId { get; set; }

    [Required]
    public int TypeId { get; set; }

    [Required]
    public int AdminId { get; set; }

    public bool IsActive { get; set; } = true;

    [Required]
    public IFormFile? Logo { get; set; }

}
