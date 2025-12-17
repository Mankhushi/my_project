namespace MSINS_API.Models.Response
{
    public class NewTestimonialsMasterResponse
    {
        public int TestimonialId { get; set; }              // PK Auto Increment
        public string? TestimonyGivenBy { get; set; }       // nvarchar(255)
        public string? TextLineTwon { get; set; }           // varchar(500)
        public string? Testimony { get; set; }              // nvarchar(max)
        public bool? IsActive { get; set; }                 // bit
        public string ProfilePic { get; set; }             // nvarchar(max)
        public int? InitiativeId { get; set; }              // FK to InitiativeMaster
    }
}
