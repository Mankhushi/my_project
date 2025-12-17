using MSINS_API.Services;
using System.Security.Cryptography;
using System.Text;

namespace JobPortalAPI.Services
{
    public class HashGenValidate : IHashGenValidate
    {
        private readonly IConfiguration _configuration;
        private readonly byte[] _secretKey;


        public HashGenValidate(IConfiguration configuration)
        {
            _configuration = configuration;
            _secretKey = Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]);

        }

        public string GenerateHash(string password)
        {
            using (var hmac = new HMACSHA256(_secretKey))
            {
                var passwordBytes = Encoding.UTF8.GetBytes(password);
                var hashedBytes = hmac.ComputeHash(passwordBytes);
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }
    }
}
