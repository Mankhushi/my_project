using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class NewSectorMasterRequest
{
    [JsonIgnore]   // body me nahi show hoga, update ke waqt use hoga
    public int? SectorId { get; set; }

    [Required]
    [StringLength(255)]
    public string SectorName { get; set; } = string.Empty;

    [Required]
    public bool IsActive { get; set; }

    [Required]
    [JsonPropertyName("adminId")]   // 👈 JSON me adminId aayega
    public int AdminId { get; set; }  // 👈 C# property name correct
}
