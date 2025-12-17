using MSINS_API.Models.Request;
using MSINS_API.Models.Response;

namespace MSINS_API.Services.Interface
{
    public interface IAboutUsService
    {
        Task<(int Code, string Message)> AddOrUpdateAboutUsAsync(AboutUsRequest aboutUsDto);

        Task<List<AboutUsResponse>> GetAboutUsAsync();
    }
}
