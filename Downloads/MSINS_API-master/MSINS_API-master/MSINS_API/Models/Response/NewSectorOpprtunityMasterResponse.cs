namespace MSINS_API.Models.Response
{
    public class NewSectorOpprtunityMasterResponse
    {
        public int SectorOpprtunityId { get; set; }          // PK

        public string? SectorOpprtunityImage { get; set; }   // URL or path of uploaded image

        public string? SectorOpprtunityName { get; set; }    // Name / description

        public bool IsActive { get; set; }                   // Active / inactive

        public int InitiativeId { get; set; }                // FK to InitiativeMaster
    }
}
