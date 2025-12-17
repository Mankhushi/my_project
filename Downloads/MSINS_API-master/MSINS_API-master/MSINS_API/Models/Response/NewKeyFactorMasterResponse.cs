namespace MSINS_API.Models.Response
{
    public class NewKeyFactorMasterResponse
    {
        public int KeyFactorId { get; set; }          // PK

        public string? KeyFactorImage { get; set; }   // URL or stored path

        public string? KeyFactorName { get; set; }    // nvarchar(255)

        public bool IsActive { get; set; }

        public int InitiativeId { get; set; }         // FK → Initiative Master
    }
}
