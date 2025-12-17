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
    public class MediaRepository : IMediaRepository
    {
        private readonly string _connectionString;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly BaseUrlSettings _baseUrlSettings;


        public MediaRepository(IHttpContextAccessor httpContextAccessor, IOptions<BaseUrlSettings> baseUrlSettings, IConfiguration configuration, IMapper mapper)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _baseUrlSettings = baseUrlSettings.Value;
        }

        public async Task<(int Code, string Message)> AddOrUpdateMediaAsync(MediaRequest MediaDto, string? fileUrl)
        {
            int statusCode = 500;
            string message = "Unknown error occurred.";

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("AddOrUpdateMedia", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@MediaId", MediaDto.MediaId);
                    command.Parameters.AddWithValue("@MediaDate", MediaDto.MediaDate);
                    command.Parameters.AddWithValue("@MediaName", MediaDto.MediaName);
                    command.Parameters.AddWithValue("@MediaLink", MediaDto.MediaLink);
                    command.Parameters.AddWithValue("@MediaDesc", MediaDto.MediaDesc);
                    command.Parameters.AddWithValue("@ImagePath", (object?)fileUrl ?? DBNull.Value);
                    command.Parameters.AddWithValue("@IsActive", MediaDto.IsActive);
                    command.Parameters.AddWithValue("@UserId", MediaDto.AdminId);

                    // Output parameters for Status Code and Message
                    var statusParam = new SqlParameter("@ResultCode", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    var messageParam = new SqlParameter("@ResultMessage", SqlDbType.NVarChar, 500)
                    {
                        Direction = ParameterDirection.Output
                    };

                    command.Parameters.Add(statusParam);
                    command.Parameters.Add(messageParam);

                    await command.ExecuteNonQueryAsync();

                    // Retrieve output values
                    statusCode = Convert.ToInt32(statusParam.Value);
                    message = messageParam.Value.ToString() ?? "No message returned.";

                    return (statusCode, message);
                }
            }
        }

        public async Task<PagedResponse<MediaResponse>> GetMediasAsync(MediaQueryParamRequest queryParams)
        {
            var Medias = new List<MediaResponse>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("GetAllMedias", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Search", (object?)queryParams.Search ?? DBNull.Value);
                    command.Parameters.AddWithValue("@IsActive", queryParams.IsActive.HasValue ? (object)queryParams.IsActive.Value : DBNull.Value);
                    command.Parameters.AddWithValue("@StartDate", (object?)queryParams.MediaStartDate ?? DBNull.Value);
                    command.Parameters.AddWithValue("@EndDate", (object?)queryParams.MediaEndDate ?? DBNull.Value);
                    command.Parameters.AddWithValue("@PageIndex", queryParams.PageIndex);
                    command.Parameters.AddWithValue("@PageSize", queryParams.PageSize);

                    var totalRecordsParam = new SqlParameter("@TotalRecords", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(totalRecordsParam);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var recordDetail = _mapper.Map<MediaResponse>(reader);

                            // Get Base URL dynamically from HttpContext
                            var request = _httpContextAccessor.HttpContext?.Request;
                            string baseUrl = request != null
                                ? $"{request.Scheme}://{request.Host}"
                                : _baseUrlSettings.Production; // Production URL from settings

                            if (!string.IsNullOrEmpty(recordDetail.ImagePath))
                            {
                                recordDetail.ImagePath = $"{baseUrl}/{recordDetail.ImagePath.TrimStart('/')}";
                            }

                            Medias.Add(recordDetail);
                        }
                    }

                    int totalRecords = totalRecordsParam.Value != DBNull.Value ? Convert.ToInt32(totalRecordsParam.Value) : 0;
                    return new PagedResponse<MediaResponse>
                    {
                        Data = Medias,
                        TotalRecords = totalRecords
                    };
                }
            }
        }
    }
}
