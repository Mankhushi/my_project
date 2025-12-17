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
    public class GovernmentResolutionRepository : IGovernmentResolutionRepository
    {
        private readonly string _connectionString;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly BaseUrlSettings _baseUrlSettings;


        public GovernmentResolutionRepository(IHttpContextAccessor httpContextAccessor, IOptions<BaseUrlSettings> baseUrlSettings, IConfiguration configuration, IMapper mapper)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _baseUrlSettings = baseUrlSettings.Value;
        }

        public async Task<(int Code, string Message)> AddOrUpdateGovernmentResolutionAsync(GovernmentResolutionRequest featuredDto, string? fileUrl)
        {
            int statusCode = 500;
            string message = "Unknown error occurred.";

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("AddOrUpdateGovernmentResolution", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@GovernmentResolutionId", featuredDto.GovernmentResolutionId);
                    command.Parameters.AddWithValue("@Title", featuredDto.Title);
                    command.Parameters.AddWithValue("@Link", (object?)fileUrl ?? DBNull.Value);
                    command.Parameters.AddWithValue("@IsActive", featuredDto.IsActive);
                    command.Parameters.AddWithValue("@UserId", featuredDto.AdminId);

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

        public async Task<PagedResponse<GovernmentResolutionResponse>> GetGovernmentResolutionAsync(GovernmentResolutionQueryParamsRequest queryParams)
        {
            var GovernmentResolutions = new List<GovernmentResolutionResponse>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("GetAllGovernmentResolutions", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Search", (object?)queryParams.Title ?? DBNull.Value);
                    command.Parameters.AddWithValue("@IsActive", queryParams.IsActive.HasValue ? (object)queryParams.IsActive.Value : DBNull.Value);
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
                            var recordDetail = _mapper.Map<GovernmentResolutionResponse>(reader);

                            // Get Base URL dynamically from HttpContext
                            var request = _httpContextAccessor.HttpContext?.Request;
                            string baseUrl = request != null
                                ? $"{request.Scheme}://{request.Host}"
                                : _baseUrlSettings.Production;       // Production URL from settings

                            if (!string.IsNullOrEmpty(recordDetail.Link))
                            {
                                recordDetail.Link = $"{baseUrl}/{recordDetail.Link.TrimStart('/')}";
                            }


                            GovernmentResolutions.Add(recordDetail);
                        }
                    }

                    int totalRecords = totalRecordsParam.Value != DBNull.Value ? Convert.ToInt32(totalRecordsParam.Value) : 0;
                    return new PagedResponse<GovernmentResolutionResponse>
                    {
                        Data = GovernmentResolutions,
                        TotalRecords = totalRecords
                    };
                }
            }
        }
    }
}
