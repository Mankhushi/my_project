using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using MSINS_API.Models.Request;
using MSINS_API.Models.Response;
using MSINS_API.POCO;
using MSINS_API.Repositories.Interface;
using MSINS_API.Services.Implementation;
using System.Data;

namespace MSINS_API.Repositories.Implementation
{
    public class NewEmailMasterRepository : INewEmailMasterRepository
    {
        private readonly string _connectionString;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly BaseUrlSettings _baseUrlSettings;

        public NewEmailMasterRepository(
            IHttpContextAccessor httpContextAccessor,
            IOptions<BaseUrlSettings> baseUrlSettings,
            IConfiguration configuration,
            IMapper mapper)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _baseUrlSettings = baseUrlSettings.Value;
        }

        // ===================== COMMON METHOD =====================
        private int GetLoggedInAdminId()
        {
            return Convert.ToInt32(
                _httpContextAccessor.HttpContext?
                    .User?
                    .FindFirst("AdminId")?.Value ?? "0"
            );
        }

        // ===================== ADD EMAIL =====================
        public async Task<(int Code, string Message, int EmailId)> AddEmailAsync(NewEmailMasterRequest request)
        {
            int userId = GetLoggedInAdminId(); // JWT / HttpContext se

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand("dbo.usp_EmailMaster_Insert", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@Email", request.Email ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@IsActive", request.IsActive);

            // ✅ THIS WAS MISSING
            command.Parameters.AddWithValue("@UserId", userId);

            var emailIdParam = new SqlParameter("@EmailId", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };

            var resultCodeParam = new SqlParameter("@ResultCode", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };

            var resultMessageParam = new SqlParameter("@ResultMessage", SqlDbType.NVarChar, 500)
            {
                Direction = ParameterDirection.Output
            };

            command.Parameters.Add(emailIdParam);
            command.Parameters.Add(resultCodeParam);
            command.Parameters.Add(resultMessageParam);

            await command.ExecuteNonQueryAsync();

            return (
                Convert.ToInt32(resultCodeParam.Value),
                resultMessageParam.Value.ToString(),
                Convert.ToInt32(emailIdParam.Value)
            );
        }


        // ===================== UPDATE EMAIL =====================
        public async Task<(int Code, string Message)> UpdateEmailAsync(int id, NewEmailMasterRequest request)
        {
            int adminId = GetLoggedInAdminId();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand("dbo.usp_EmailMaster_Update", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@EmailId", id);
            command.Parameters.AddWithValue("@Email",
                string.IsNullOrWhiteSpace(request.Email) ? (object)DBNull.Value : request.Email);
            command.Parameters.AddWithValue("@IsActive", request.IsActive);
            command.Parameters.AddWithValue("@AdminId", adminId);

            var resultCodeParam = new SqlParameter("@ResultCode", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };

            var resultMessageParam = new SqlParameter("@ResultMessage", SqlDbType.NVarChar, 500)
            {
                Direction = ParameterDirection.Output
            };

            command.Parameters.Add(resultCodeParam);
            command.Parameters.Add(resultMessageParam);

            await command.ExecuteNonQueryAsync();

            return (
                Convert.ToInt32(resultCodeParam.Value),
                resultMessageParam.Value.ToString()
            );
        }

        // ===================== GET EMAIL BY ID =====================
        public async Task<NewEmailMasterResponse?> GetEmailByIdAsync(int id)
        {
            int adminId = GetLoggedInAdminId();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand("dbo.usp_EmailMaster_GetById", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@EmailId", id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new NewEmailMasterResponse
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Email = reader["Email"].ToString(),
                    IsActive = Convert.ToBoolean(reader["IsActive"]),
                    adminId = adminId   // ✅ JWT se
                };
            }

            return null;
        }

        // ===================== GET ALL EMAILS (PAGINATED) =====================
        public async Task<PagedResponse<NewEmailMasterResponse>> GetAllEmailsAsync(
            int pageNumber,
            int pageSize,
            string? searchTerm,
            bool? isActive)
        {
            int adminId = GetLoggedInAdminId();
            var emails = new List<NewEmailMasterResponse>();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand("dbo.usp_EmailMaster_GetAll", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@PageNumber", pageNumber);
            command.Parameters.AddWithValue("@PageSize", pageSize);
            command.Parameters.AddWithValue("@SearchTerm",
                string.IsNullOrWhiteSpace(searchTerm) ? (object)DBNull.Value : searchTerm);
            command.Parameters.AddWithValue("@IsActive",
                isActive.HasValue ? (object)isActive.Value : DBNull.Value);

            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                emails.Add(new NewEmailMasterResponse
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Email = reader["Email"].ToString(),
                    IsActive = Convert.ToBoolean(reader["IsActive"]),
                    adminId = adminId   // ✅ JWT se
                });
            }

            int totalRecords = 0;
            int totalPages = 0;

            if (await reader.NextResultAsync() && await reader.ReadAsync())
            {
                totalRecords = Convert.ToInt32(reader["TotalCount"]);
                totalPages = Convert.ToInt32(reader["TotalPages"]);
            }

            return new PagedResponse<NewEmailMasterResponse>
            {
                Data = emails,
                TotalRecords = totalRecords,
                TotalPages = totalPages,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Message = emails.Count > 0 ? "Emails fetched successfully." : "No data found."
            };
        }

        // ===================== GET ALL (QUERY PARAMS) =====================
        public async Task<PagedResponse<NewEmailMasterResponse>> GetAllEmailsAsync(
            NewEmailMasterQueryParams queryParams)
        {
            return await GetAllEmailsAsync(
                queryParams.PageNumber,
                queryParams.PageSize,
                queryParams.SearchTerm,
                queryParams.IsActive
            );
        }

        // ===================== EXPORT EMAILS =====================
        public async Task<List<NewEmailMasterResponse>> ExportEmailsAsync(bool? isActive)
        {
            int adminId = GetLoggedInAdminId();
            var emails = new List<NewEmailMasterResponse>();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand("usp_EmailMaster_Export", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@IsActive", isActive ?? (object)DBNull.Value);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                emails.Add(new NewEmailMasterResponse
                {
                    Email = reader["Email"].ToString(),
                    IsActive = Convert.ToBoolean(reader["IsActive"]),
                    adminId = adminId   // ✅ JWT se
                });
            }

            return emails;
        }
    }
}
