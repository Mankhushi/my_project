namespace MSINS_API.Models.Response
{
    public class NewInitiativePartnersMasterResponse
    {
        public int PartnerId { get; set; }
        public string? PartnerType { get; set; }
        public string? PartnerImage { get; set; }
        public bool IsActive { get; set; }
        public int InitiativeId { get; set; }
    }
}
