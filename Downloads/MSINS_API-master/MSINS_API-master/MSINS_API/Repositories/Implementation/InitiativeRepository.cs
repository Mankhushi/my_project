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
    public class InitiativeRepository : IInitiativeRepository
    {
        private readonly string _connectionString;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly BaseUrlSettings _baseUrlSettings;

        public InitiativeRepository(IHttpContextAccessor httpContextAccessor, IOptions<BaseUrlSettings> baseUrlSettings, IConfiguration configuration, IMapper mapper)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _baseUrlSettings = baseUrlSettings.Value;
        }

        public async Task<(int Code, string Message)> AddOrUpdateInitiativeAsync(InitiativeRequest initiativeDto, string? fileUrl)
        {
            int statusCode = 500;
            string message = "Unknown error occurred.";

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("AddOrUpdateInitiative", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@InitiativeId", (object?)initiativeDto.InitiativeId ?? DBNull.Value);
                    command.Parameters.AddWithValue("@InitiativeName", initiativeDto.InitiativeName);
                    command.Parameters.AddWithValue("@InitiativeDesc", (object?)initiativeDto.InitiativeDesc ?? DBNull.Value);
                    command.Parameters.AddWithValue("@InitiativeLink", (object?)initiativeDto.InitiativeLink ?? DBNull.Value);
                    command.Parameters.AddWithValue("@LinkType", (object?)initiativeDto.LinkType ?? DBNull.Value);
                    command.Parameters.AddWithValue("@ImagePath", (object?)fileUrl ?? DBNull.Value);
                    command.Parameters.AddWithValue("@IsActive", initiativeDto.IsActive);
                    command.Parameters.AddWithValue("@UserId", initiativeDto.AdminId);

                    var statusParam = new SqlParameter("@ResultCode", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    var messageParam = new SqlParameter("@ResultMessage", SqlDbType.NVarChar, 500) { Direction = ParameterDirection.Output };

                    command.Parameters.Add(statusParam);
                    command.Parameters.Add(messageParam);

                    await command.ExecuteNonQueryAsync();

                    statusCode = Convert.ToInt32(statusParam.Value);
                    message = messageParam.Value.ToString() ?? "No message returned.";

                    return (statusCode, message);
                }
            }
        }


        public async Task<PagedResponse<InitiativeResponse>> GetInitiativeAsync(InitiativeQueryParamsRequest queryParams)
        {
            var initiatives = new List<InitiativeResponse>();
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("GetAllInitiative", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@SearchTerm", (object?)queryParams.InitiativeName ?? DBNull.Value);
                    command.Parameters.AddWithValue("@isActive", queryParams.IsActive.HasValue ? (object)queryParams.IsActive.Value : DBNull.Value);
                    command.Parameters.AddWithValue("@pageIndex", queryParams.PageIndex);
                    command.Parameters.AddWithValue("@pageSize", queryParams.PageSize);

                    var totalRecordsParam = new SqlParameter("@totalRecords", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    command.Parameters.Add(totalRecordsParam);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var recordDetail = _mapper.Map<InitiativeResponse>(reader);

                            var request = _httpContextAccessor.HttpContext?.Request;
                            string baseUrl = request != null ? $"{request.Scheme}://{request.Host}" : _baseUrlSettings.Production;

                            if (!string.IsNullOrEmpty(recordDetail.ImagePath))
                            {
                                recordDetail.ImagePath = $"{baseUrl}/{recordDetail.ImagePath.TrimStart('/')}";
                            }

                            initiatives.Add(recordDetail);
                        }
                    }
                    int totalRecords = totalRecordsParam.Value != DBNull.Value ? Convert.ToInt32(totalRecordsParam.Value) : 0;
                    return new PagedResponse<InitiativeResponse>
                    {
                        Data = initiatives,
                        TotalRecords = totalRecords
                    };
                }
            }
        }
    }
}


