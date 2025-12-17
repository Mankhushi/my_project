namespace MSINS_API.POCO
{
    public class JwtSettings
    {
        public string SecretKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string[] ValidIssuers { get; set; }
        public string[] ValidAudiences { get; set; }
        public int DurationInMinutes { get; set; }
        public int GuestDurationInMinutes { get; set; }
        public int RefreshGuestDurationInMinutes { get; set; }
        public int RefreshDurationInMinutes { get; set; }
    }
}
