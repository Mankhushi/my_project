namespace MSINS_API.Models.Response
{
    public class AuthResponse
    {
        public string? Token { get; set; }
        public string? ErrorMessage { get; set; }
        public bool IsSuccess { get; set; }
        public DateTime? Expiration { get; set; }
        public string? RefreshToken { get; set; }
    }
}
