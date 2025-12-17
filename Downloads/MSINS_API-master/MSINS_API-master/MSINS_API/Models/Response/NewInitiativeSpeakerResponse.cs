namespace MSINS_API.Models.Response
{
    public class NewInitiativeSpeakerResponse
    {
        public int SpeakerId { get; set; }     
        public string Name { get; set; }
        public string Designation { get; set; }
        public string ProfilePicUrl { get; set; }
        public bool IsActive { get; set; }
    }
}
