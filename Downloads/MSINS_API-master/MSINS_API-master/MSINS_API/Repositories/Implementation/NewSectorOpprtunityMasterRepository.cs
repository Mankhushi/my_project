using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using MSINS_API.Models.Request;
using MSINS_API.Models.Response;
using MSINS_API.POCO;
using MSINS_API.Repositories.Interface;
using System.Data;

namespace MSINS_API.Repositories.Implementation
{
    public class NewSectorOpprtunityMasterRepository : INewSectorOpprtunityMasterRepository
    {
        private readonly string _connectionString;

        public NewSectorOpprtunityMasterRepository(
            IHttpContextAccessor httpContextAccessor,
            IOptions<BaseUrlSettings> baseUrlSettings,
            IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // ============================================================
        //                     ADD SECTOR OPPORTUNITY
        // ============================================================
        public async Task<(int Code, string Message)> AddSectorOpprtunityAsync(
            NewSectorOpprtunityMasterRequest request,
            string fileUrl)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var cmd = new SqlCommand("usp_SectorOpprtunity_Insert", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@SectorOpprtunityImage", (object?)fileUrl ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@SectorOpprtunityName", request.SectorOpprtunityName);
            cmd.Parameters.AddWithValue("@IsActive", request.IsActive);
            cmd.Parameters.AddWithValue("@InitiativeId", request.InitiativeId);
            cmd.Parameters.AddWithValue("@AdminId", request.AdminId);

            var resultCode = new SqlParameter("@ResultCode", SqlDbType.Int)
            { Direction = ParameterDirection.Output };

            var resultMsg = new SqlParameter("@ResultMessage", SqlDbType.NVarChar, 500)
            { Direction = ParameterDirection.Output };

            cmd.Parameters.Add(resultCode);
            cmd.Parameters.Add(resultMsg);

            await cmd.ExecuteNonQueryAsync();

            return (
                Convert.ToInt32(resultCode.Value),
                Convert.ToString(resultMsg.Value) ?? "Operation completed."
            );
        }

        // ============================================================
        //                     UPDATE SECTOR OPPORTUNITY
        // ============================================================
        public async Task<(int Code, string Message)> UpdateSectorOpprtunityAsync(
            int sectorOpprtunityId,
            NewSectorOpprtunityMasterRequest request,
            string? fileUrl)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var cmd = new SqlCommand("usp_SectorOpprtunity_Update", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@SectorOpprtunityId", sectorOpprtunityId);
            cmd.Parameters.AddWithValue("@SectorOpprtunityImage", (object?)fileUrl ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@SectorOpprtunityName", request.SectorOpprtunityName);
            cmd.Parameters.AddWithValue("@IsActive", request.IsActive);
            cmd.Parameters.AddWithValue("@InitiativeId", request.InitiativeId);
            cmd.Parameters.AddWithValue("@AdminId", request.AdminId);

            var resultCode = new SqlParameter("@ResultCode", SqlDbType.Int)
            { Direction = ParameterDirection.Output };

            var resultMsg = new SqlParameter("@ResultMessage", SqlDbType.NVarChar, 500)
            { Direction = ParameterDirection.Output };

            cmd.Parameters.Add(resultCode);
            cmd.Parameters.Add(resultMsg);

            await cmd.ExecuteNonQueryAsync();

            return (
                Convert.ToInt32(resultCode.Value),
                Convert.ToString(resultMsg.Value) ?? "Operation completed."
            );
        }

        // ============================================================
        //                     GET ALL
        // ============================================================
        public async Task<List<NewSectorOpprtunityMasterResponse>> GetSectorOpprtunityAsync(
            bool? isActive,
            int? initiativeId)
        {
            var list = new List<NewSectorOpprtunityMasterResponse>();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var cmd = new SqlCommand("usp_SectorOpprtunity_GetAll", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@IsActive", (object?)isActive ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@InitiativeId", (object?)initiativeId ?? DBNull.Value);

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new NewSectorOpprtunityMasterResponse
                {
                    SectorOpprtunityId = Convert.ToInt32(reader["SectorOpprtunityId"]),
                    SectorOpprtunityName = reader["SectorOpprtunityName"].ToString(),
                    SectorOpprtunityImage = reader["SectorOpprtunityImage"]?.ToString(), // ONLY RELATIVE PATH
                    IsActive = Convert.ToBoolean(reader["IsActive"]),
                    InitiativeId = Convert.ToInt32(reader["InitiativeId"])
                });
            }

            return list;
        }

        // ============================================================
        //                     GET BY ID
        // ============================================================
        public async Task<NewSectorOpprtunityMasterResponse?> GetSectorOpprtunityByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var cmd = new SqlCommand("usp_SectorOpprtunity_GetById", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@SectorOpprtunityId", id);

            using var reader = await cmd.ExecuteReaderAsync();
            if (!await reader.ReadAsync())
                return null;

            return new NewSectorOpprtunityMasterResponse
            {
                SectorOpprtunityId = Convert.ToInt32(reader["SectorOpprtunityId"]),
                SectorOpprtunityName = reader["SectorOpprtunityName"].ToString(),
                SectorOpprtunityImage = reader["SectorOpprtunityImage"]?.ToString(), // RELATIVE PATH
                IsActive = Convert.ToBoolean(reader["IsActive"]),
                InitiativeId = Convert.ToInt32(reader["InitiativeId"])
            };
        }
    }
}
