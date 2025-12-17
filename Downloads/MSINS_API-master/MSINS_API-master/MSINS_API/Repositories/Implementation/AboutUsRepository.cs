using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using MSINS_API.Models.Request;
using MSINS_API.Models.Response;
using MSINS_API.POCO;
using MSINS_API.Repositories.Interface;
using System.Data;

namespace MSINS_API.Repositories.Implementation
{
    public class AboutUsRepository : IAboutUsRepository
    {
        private readonly string _connectionString;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly BaseUrlSettings _baseUrlSettings;


        public AboutUsRepository(IHttpContextAccessor httpContextAccessor, IOptions<BaseUrlSettings> baseUrlSettings, IConfiguration configuration, IMapper mapper)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _baseUrlSettings = baseUrlSettings.Value;
        }

        public async Task<(int Code, string Message)> AddOrUpdateAboutUsAsync(AboutUsRequest aboutUsDto, string? fileUrl, string? pdfUrl)
        {
            int resultCode = 500;
            string resultMessage;

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("UpdateAboutUs", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@TextOne", aboutUsDto.TextOne);
                    command.Parameters.AddWithValue("@TextTwo", aboutUsDto.TextTwo);
                    command.Parameters.AddWithValue("@ImagePath", (object?)fileUrl ?? DBNull.Value);
                    command.Parameters.AddWithValue("@PDFPath", (object?)pdfUrl ?? DBNull.Value);
                    command.Parameters.AddWithValue("@UserId", aboutUsDto.adminId);

                    // Output parameters
                    var resultCodeParam = new SqlParameter("@ResultCode", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    var resultMessageParam = new SqlParameter("@ResultMessage", SqlDbType.NVarChar, 500) { Direction = ParameterDirection.Output };
                    command.Parameters.Add(resultCodeParam);
                    command.Parameters.Add(resultMessageParam);

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();

                    resultCode = (int)resultCodeParam.Value;
                    resultMessage = (string)resultMessageParam.Value;
                }
            }
            return (resultCode, resultMessage);
        }

        public async Task<List<AboutUsResponse>> GetAboutUsAsync()
        {
            var aboutUs = new List<AboutUsResponse>();
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("GetAboutUs", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;


                    using (var reader = await command.ExecuteReaderAsync())
                    {

                        while (await reader.ReadAsync())
                        {
                            // Map the current reader row to the RecruiterListEFModel using AutoMapper
                            var recordDetail = _mapper.Map<AboutUsResponse>(reader);

                            // Get Base URL dynamically from HttpContext
                            var request = _httpContextAccessor.HttpContext?.Request;
                            string baseUrl = request != null
                                ? $"{request.Scheme}://{request.Host}"
                                : _baseUrlSettings.Production;       // Production URL from settings


                            if (!string.IsNullOrEmpty(recordDetail.ImageFile))
                            {
                                recordDetail.ImageFile = $"{baseUrl}/{recordDetail.ImageFile.TrimStart('/')}";
                            }

                            if (!string.IsNullOrEmpty(recordDetail.PDFFile))
                            {
                                recordDetail.PDFFile = $"{baseUrl}/{recordDetail.PDFFile.TrimStart('/')}";
                            }

                            aboutUs.Add(recordDetail); // Add the mapped recruiter to the list


                        }

                    }
                    return aboutUs;
                }
            }
        }
    }
}
