using MSINS_API.Models.Response;
public class NewIncubatorsMasterResponse
{
    public int IncubatorId { get; set; }
    public string IncubatorName { get; set; }
    public string CityName { get; set; }
    public string SectorName { get; set; }
    public string TypeName { get; set; }
    public string LogoPath { get; set; }
    public bool IsActive { get; set; }
}

public class IncubatorsResultResponse
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
    public int IncubatorId { get; set; }
}


