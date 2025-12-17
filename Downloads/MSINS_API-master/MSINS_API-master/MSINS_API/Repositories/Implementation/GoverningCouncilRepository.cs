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
    public class GoverningCouncilRepository : IGoverningCouncilRepository
    {
        private readonly string _connectionString;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly BaseUrlSettings _baseUrlSettings;


        public GoverningCouncilRepository(IHttpContextAccessor httpContextAccessor, IOptions<BaseUrlSettings> baseUrlSettings, IConfiguration configuration, IMapper mapper)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _baseUrlSettings = baseUrlSettings.Value;
        }
        public async Task<(int Code, string Message)> AddOrUpdateCouncilAsync(GoverningCouncilRequest councilDto)
        {
            int statusCode = 500;
            string message = "Unknown error occurred.";

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("AddOrUpdateGoverningCouncil", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@GoverningCouncilId", councilDto.GoverningCouncilId);
                    command.Parameters.AddWithValue("@Designation", councilDto.Designation);
                    command.Parameters.AddWithValue("@Address", councilDto.Address);
                    command.Parameters.AddWithValue("@Position", councilDto.Position);
                    command.Parameters.AddWithValue("@Name", councilDto.Name);
                    command.Parameters.AddWithValue("@IsActive", councilDto.IsActive);
                    command.Parameters.AddWithValue("@UserId", councilDto.adminId);

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

        public async Task<PagedResponse<GoverningCouncilResponse>> GetCouncilAsync(GoverningCouncilQueryParamRequest queryParams)
        {
            var council = new List<GoverningCouncilResponse>();
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("GetAllGoverningCouncil", connection))
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
                            var recordDetail = _mapper.Map<GoverningCouncilResponse>(reader);

                            council.Add(recordDetail); // Add the mapped recruiter to the list


                        }

                    }
                    int totalRecords = totalRecordsParam.Value != DBNull.Value ? Convert.ToInt32(totalRecordsParam.Value) : 0;
                    return new PagedResponse<GoverningCouncilResponse>
                    {
                        Data = council,
                        TotalRecords = totalRecords
                    };
                }
            }
        }
    }
}
