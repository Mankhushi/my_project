using MSINS_API.Models;

namespace MSINS_API.Services
{
    public interface IHashGenValidate
    {
        string GenerateHash(string password);
    }
}
