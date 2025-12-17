using MSINS_API.Models.Request;
using MSINS_API.Models.Response;

namespace MSINS_API.Repositories.Interface
{
    public interface IAboutUsRepository
    {
        Task<(int Code, string Message)> AddOrUpdateAboutUsAsync(AboutUsRequest abputDto, string? fileUrl, string? pdfUrl);

        Task<List<AboutUsResponse>> GetAboutUsAsync();
    }
}
