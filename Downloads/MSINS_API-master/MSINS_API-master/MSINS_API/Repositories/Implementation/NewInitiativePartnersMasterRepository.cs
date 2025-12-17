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
    public class NewInitiativePartnersMasterRepository : INewInitiativePartnersMasterRepository
    {
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly BaseUrlSettings _baseUrlSettings;

        public NewInitiativePartnersMasterRepository(
            IHttpContextAccessor httpContextAccessor,
            IOptions<BaseUrlSettings> baseUrlSettings,
            IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _httpContextAccessor = httpContextAccessor;
            _baseUrlSettings = baseUrlSettings.Value;
        }

        // ============================================================
        //                     ADD PARTNER
        // ============================================================
        public async Task<(int Code, string Message)> AddPartnerAsync(
            NewInitiativePartnersMasterRequest request,
            string? fileUrl)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand("usp_Partners_Insert", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@PartnerType", request.PartnerType ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@PartnerImage", fileUrl ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@IsActive", request.IsActive);
            command.Parameters.AddWithValue("@InitiativeId", request.InitiativeId);
            command.Parameters.AddWithValue("@AdminId", request.AdminId);

            var resultCodeParam = new SqlParameter("@ResultCode", SqlDbType.Int)
            { Direction = ParameterDirection.Output };

            var resultMessageParam = new SqlParameter("@ResultMessage", SqlDbType.NVarChar, 500)
            { Direction = ParameterDirection.Output };

            command.Parameters.Add(resultCodeParam);
            command.Parameters.Add(resultMessageParam);

            await command.ExecuteNonQueryAsync();

            return (
                Convert.ToInt32(resultCodeParam.Value),
                Convert.ToString(resultMessageParam.Value) ?? "Operation completed."
            );
        }

        // ============================================================
        //                     UPDATE PARTNER
        // ============================================================
        public async Task<(int Code, string Message)> UpdatePartnerAsync(
            int partnerId,
            NewInitiativePartnersMasterRequest request,
            string? fileUrl)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand("usp_Partners_Update", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@PartnerId", partnerId);
            command.Parameters.AddWithValue("@PartnerType", request.PartnerType ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@PartnerImage", fileUrl ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@IsActive", request.IsActive);
            command.Parameters.AddWithValue("@InitiativeId", request.InitiativeId);
            command.Parameters.AddWithValue("@AdminId", request.AdminId);

            var resultCodeParam = new SqlParameter("@ResultCode", SqlDbType.Int)
            { Direction = ParameterDirection.Output };

            var resultMessageParam = new SqlParameter("@ResultMessage", SqlDbType.NVarChar, 500)
            { Direction = ParameterDirection.Output };

            command.Parameters.Add(resultCodeParam);
            command.Parameters.Add(resultMessageParam);

            await command.ExecuteNonQueryAsync();

            return (
                Convert.ToInt32(resultCodeParam.Value),
                Convert.ToString(resultMessageParam.Value) ?? "Operation completed."
            );
        }

        // ============================================================
        //                    GET ALL PARTNERS
        // ============================================================
        public async Task<List<NewInitiativePartnersMasterResponse>> GetPartnersAsync(
            int? initiativeId,
            bool? isActive)
        {
            var list = new List<NewInitiativePartnersMasterResponse>();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand("usp_Partners_GetAll", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@InitiativeId", initiativeId ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@IsActive", isActive ?? (object)DBNull.Value);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                string? img = reader["PartnerImage"]?.ToString();
                string? fullUrl = !string.IsNullOrEmpty(img)
                    ? $"{_baseUrlSettings.Production}/{img}".Replace("\\", "/")
                    : null;

                list.Add(new NewInitiativePartnersMasterResponse
                {
                    PartnerId = Convert.ToInt32(reader["PartnerId"]),
                    PartnerType = reader["PartnerType"].ToString(),
                    PartnerImage = fullUrl,     // FINAL FULL URL
                    IsActive = Convert.ToBoolean(reader["IsActive"]),
                    InitiativeId = Convert.ToInt32(reader["InitiativeId"])
                });
            }

            return list;
        }

        // ============================================================
        //                 GET PARTNER BY ID
        // ============================================================
        public async Task<NewInitiativePartnersMasterResponse?> GetPartnerByIdAsync(int partnerId)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand("usp_Partners_GetById", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@PartnerId", partnerId);

            using var reader = await command.ExecuteReaderAsync();
            if (!await reader.ReadAsync())
                return null;

            string? img = reader["PartnerImage"]?.ToString();
            string? fullUrl = !string.IsNullOrEmpty(img)
                ? $"{_baseUrlSettings.Production}/{img}".Replace("\\", "/")
                : null;

            return new NewInitiativePartnersMasterResponse
            {
                PartnerId = Convert.ToInt32(reader["PartnerId"]),
                PartnerType = reader["PartnerType"].ToString(),
                PartnerImage = fullUrl,
                IsActive = Convert.ToBoolean(reader["IsActive"]),
                InitiativeId = Convert.ToInt32(reader["InitiativeId"])
            };
        }
    }
}
