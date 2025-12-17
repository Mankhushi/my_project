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
    public class NewInitiativeMasterRepository : INewInitiativeMasterRepository
    {
        private readonly string _connectionString;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly BaseUrlSettings _baseUrlSettings;

        public NewInitiativeMasterRepository(
            IHttpContextAccessor httpContextAccessor,
            IOptions<BaseUrlSettings> baseUrlSettings,
            IConfiguration configuration,
            IMapper mapper)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _baseUrlSettings = baseUrlSettings.Value;
        }

        // ------------------------------------------------------------
        // ⭐ ADD INITIATIVE
        // ------------------------------------------------------------
        public async Task<(int Code, string Message)> AddInitiativeMasterAsync(
            NewInitiativeMasterRequest request,
            string? imageUrl)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand("AddInitiativeMaster", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@Title", request.Title ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@HeaderImage", (object?)imageUrl ?? DBNull.Value);
            command.Parameters.AddWithValue("@Brief", request.Brief ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Schedule", request.Schedule ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Eligibility", request.Eligibility ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Benefits", request.Benefits ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Milestone", request.Milestone ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Description", request.Description ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@IsActive", request.IsActive);
            //command.Parameters.AddWithValue("@AdminId", request.AdminId);

            // 🆕 NEW FIELDS
            command.Parameters.AddWithValue("@ApplyNowLink", request.ApplyNowLink ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@ReachOutEmail", request.ReachOutEmail ?? (object)DBNull.Value);

            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("userId")?.Value;
            int userId = string.IsNullOrEmpty(userIdClaim) ? 0 : int.Parse(userIdClaim);

            command.Parameters.AddWithValue("@UserId", userId);

            var codeParam = new SqlParameter("@ResultCode", SqlDbType.Int) { Direction = ParameterDirection.Output };
            var msgParam = new SqlParameter("@ResultMessage", SqlDbType.NVarChar, 500) { Direction = ParameterDirection.Output };

            command.Parameters.Add(codeParam);
            command.Parameters.Add(msgParam);

            await command.ExecuteNonQueryAsync();

            return (
                Convert.ToInt32(codeParam.Value),
                msgParam.Value?.ToString() ?? "Operation completed."
            );
        }

        // ------------------------------------------------------------
        // ⭐ UPDATE INITIATIVE
        // ------------------------------------------------------------
        public async Task<(int Code, string Message)> UpdateInitiativeMasterAsync(
            int id,
            NewInitiativeMasterRequest request,
            string? imageUrl)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand("UpdateInitiativeMaster", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@Id", id);
            command.Parameters.AddWithValue("@Title", request.Title ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@HeaderImage", (object?)imageUrl ?? DBNull.Value);
            command.Parameters.AddWithValue("@Brief", request.Brief ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Schedule", request.Schedule ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Eligibility", request.Eligibility ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Benefits", request.Benefits ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Milestone", request.Milestone ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Description", request.Description ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@IsActive", request.IsActive);
            //command.Parameters.AddWithValue("@AdminId", request.AdminId);

            // 🆕 NEW FIELDS
            command.Parameters.AddWithValue("@ApplyNowLink", request.ApplyNowLink ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@ReachOutEmail", request.ReachOutEmail ?? (object)DBNull.Value);

            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("userId")?.Value;
            int userId = string.IsNullOrEmpty(userIdClaim) ? 0 : int.Parse(userIdClaim);

            command.Parameters.AddWithValue("@UserId", userId);

            var codeParam = new SqlParameter("@ResultCode", SqlDbType.Int) { Direction = ParameterDirection.Output };
            var msgParam = new SqlParameter("@ResultMessage", SqlDbType.NVarChar, 500) { Direction = ParameterDirection.Output };

            command.Parameters.Add(codeParam);
            command.Parameters.Add(msgParam);

            await command.ExecuteNonQueryAsync();

            return (
                Convert.ToInt32(codeParam.Value),
                msgParam.Value?.ToString() ?? "Operation completed."
            );
        }

        // ------------------------------------------------------------
        // ⭐ GET BY ID
        // ------------------------------------------------------------
        public async Task<NewInitiativeMasterResponse?> GetInitiativeMasterByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand("GetInitiativeMasterById", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@Id", id);

            using var reader = await command.ExecuteReaderAsync();
            if (!reader.Read()) return null;

            var result = new NewInitiativeMasterResponse
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Title = reader["Title"]?.ToString(),
                HeaderImage = reader["HeaderImage"]?.ToString(),
                Brief = reader["Brief"]?.ToString(),
                Schedule = reader["Schedule"]?.ToString(),
                Eligibility = reader["Eligibility"]?.ToString(),
                Benefits = reader["Benefits"]?.ToString(),
                Milestone = reader["Milestone"]?.ToString(),
                Description = reader["Description"]?.ToString(),
                IsActive = Convert.ToBoolean(reader["IsActive"]),

                // 🆕 NEW FIELDS
                ApplyNowLink = reader["ApplyNowLink"]?.ToString(),
                ReachOutEmail = reader["ReachOutEmail"]?.ToString()
            };

            // Convert relative to absolute URL
            var request = _httpContextAccessor.HttpContext?.Request;
            string baseUrl = request != null
                ? $"{request.Scheme}://{request.Host}"
                : _baseUrlSettings.Production;

            if (!string.IsNullOrEmpty(result.HeaderImage))
                result.HeaderImage = $"{baseUrl}/{result.HeaderImage.TrimStart('/')}";

            return result;
        }

        // ------------------------------------------------------------
        // ⭐ GET LIST
        // ------------------------------------------------------------
        public async Task<(List<NewInitiativeMasterResponse> Data, int TotalRecords)>
    GetInitiativeMasterAsync(string? title, bool? isActive, int pageIndex, int pageSize)
        {
            var list = new List<NewInitiativeMasterResponse>();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand("GetAllInitiativeMaster", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@Title", (object?)title ?? DBNull.Value);
            command.Parameters.AddWithValue("@IsActive", (object?)isActive ?? DBNull.Value);
            command.Parameters.AddWithValue("@PageIndex", pageIndex);
            command.Parameters.AddWithValue("@PageSize", pageSize);

            var totalRecordsParam = new SqlParameter("@TotalRecords", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };

            command.Parameters.Add(totalRecordsParam);

            // 🔥 First read result set
            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    list.Add(new NewInitiativeMasterResponse
                    {
                        Id = reader.GetInt32(0),
                        Title = reader["Title"].ToString(),
                        HeaderImage = reader["HeaderImage"].ToString(),
                        Brief = reader["Brief"].ToString(),
                        Schedule = reader["Schedule"].ToString(),
                        Eligibility = reader["Eligibility"].ToString(),
                        Benefits = reader["Benefits"].ToString(),
                        Milestone = reader["Milestone"].ToString(),
                        Description = reader["Description"].ToString(),
                        IsActive = Convert.ToBoolean(reader["IsActive"])
                    });
                }
            }

            // 🔥 After reader closes → NOW read output parameter
            int totalRecords = totalRecordsParam.Value != DBNull.Value
                ? Convert.ToInt32(totalRecordsParam.Value)
                : 0;

            return (list, totalRecords);
        }

    }
}
