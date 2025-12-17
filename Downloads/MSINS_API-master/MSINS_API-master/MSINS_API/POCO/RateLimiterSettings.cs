namespace MSINS_API.POCO
{
    public class RateLimiterSettings
    {
        public int PermitLimit { get; set; }
        public int WindowSeconds { get; set; }
        public int QueueLimit { get; set; }
        public int RejectionStatusCode { get; set; }
        public int RetryAfter { get; set; }
    }
}
