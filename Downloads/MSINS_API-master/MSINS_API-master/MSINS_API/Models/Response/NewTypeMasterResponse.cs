using System.Text.Json.Serialization;

public class NewTypeMasterResponse
{

    public int TypeId { get; set; }
    public string TypeName { get; set; }
    public string SectorName { get; set; }
    public bool IsActive { get; set; }
}
