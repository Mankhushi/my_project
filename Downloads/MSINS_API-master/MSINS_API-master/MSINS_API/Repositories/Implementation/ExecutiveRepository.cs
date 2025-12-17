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
    public class ExecutiveRepository: IExecutiveRepository
    {
        private readonly string _connectionString;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly BaseUrlSettings _baseUrlSettings;

        public ExecutiveRepository(IHttpContextAccessor httpContextAccessor, IOptions<BaseUrlSettings> baseUrlSettings, IConfiguration configuration, IMapper mapper)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _baseUrlSettings = baseUrlSettings.Value;
        }

        public async Task<(int Code, string Message)> AddOrUpdateExecutiveAsync(ExecutiveRequest executiveDto, string? fileUrl)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("AddOrUpdateExecutive", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@ExecutiveId", executiveDto.ExecutiveId ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@ImagePath", (object?)fileUrl ?? DBNull.Value);
                    command.Parameters.AddWithValue("@IsActive", executiveDto.IsActive);
                    command.Parameters.AddWithValue("@ExecutiveName", executiveDto.ExecutiveName);
                    command.Parameters.AddWithValue("@ExecutiveDesc", (object?)executiveDto.ExecutiveDesc ?? DBNull.Value);
                    command.Parameters.AddWithValue("@UserId", executiveDto.AdminId);

                    var statusParam = new SqlParameter("@ResultCode", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    var messageParam = new SqlParameter("@ResultMessage", SqlDbType.NVarChar, 500) { Direction = ParameterDirection.Output };
                    command.Parameters.Add(statusParam);
                    command.Parameters.Add(messageParam);

                    await command.ExecuteNonQueryAsync();

                    return (Convert.ToInt32(statusParam.Value), messageParam.Value.ToString() ?? "No message returned.");
                }
            }
        }

        public async Task<PagedResponse<ExecutiveResponse>> GetExecutiveAsync(ExecutiveQueryParamsRequest queryParams)
        {
            var executives = new List<ExecutiveResponse>();
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("GetAllExecutives", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Search", (object?)queryParams.Search ?? DBNull.Value);
                    command.Parameters.AddWithValue("@isActive", queryParams.IsActive.HasValue ? (object)queryParams.IsActive.Value : DBNull.Value);
                    command.Parameters.AddWithValue("@pageIndex", queryParams.PageIndex);
                    command.Parameters.AddWithValue("@pageSize", queryParams.PageSize);

                    var totalRecordsParam = new SqlParameter("@totalRecords", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(totalRecordsParam);


                    using (var reader = await command.ExecuteReaderAsync())
                    {

                        while (await reader.ReadAsync())
                        {
                            // Map the current reader row to the RecruiterListEFModel using AutoMapper
                            var recordDetail = _mapper.Map<ExecutiveResponse>(reader);

                            // Get Base URL dynamically from HttpContext
                            var request = _httpContextAccessor.HttpContext?.Request;
                            string baseUrl = request != null
                                ? $"{request.Scheme}://{request.Host}"
                                : _baseUrlSettings.Production;       // Production URL from settings


                            if (!string.IsNullOrEmpty(recordDetail.ImagePath))
                            {
                                recordDetail.ImagePath = $"{baseUrl}/{recordDetail.ImagePath.TrimStart('/')}";
                            }

                            executives.Add(recordDetail); // Add the mapped recruiter to the list


                        }

                    }
                    int totalRecords = totalRecordsParam.Value != DBNull.Value ? Convert.ToInt32(totalRecordsParam.Value) : 0;
                    return new PagedResponse<ExecutiveResponse>
                    {
                        Data = executives,
                        TotalRecords = totalRecords
                    };
                }
            }
        }
    }
}

