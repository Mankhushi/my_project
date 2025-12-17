
namespace MSINS_API.Models.Response
{
    public class NewSpeakerMasterResponse
    {
        public int SpeakerId { get; set; }
        public string SpeakerName { get; set; }
        public string Designation { get; set; }
        public string Organization { get; set; }
        public string? ProfilePic { get; set; }
        public bool IsActive { get; set; }
        public string? AdminId { get; internal set; }
        public int InitiativeId { get; set; }
    }
}
