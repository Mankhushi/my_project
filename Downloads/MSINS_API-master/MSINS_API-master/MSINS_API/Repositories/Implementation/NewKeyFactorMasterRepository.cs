using Microsoft.Data.SqlClient;
using MSINS_API.Models.Request;
using MSINS_API.Models.Response;
using MSINS_API.Repositories.Interface;
using System.Data;

namespace MSINS_API.Repositories.Implementation
{
    public class NewKeyFactorMasterRepository : INewKeyFactorMasterRepository
    {
        private readonly string _connectionString;

        public NewKeyFactorMasterRepository(
            IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // ============================================================
        //                         ADD KEY FACTOR
        // ============================================================
        public async Task<(int Code, string Message)> AddKeyFactorAsync(
            NewKeyFactorMasterRequest request,
            string fileUrl)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var cmd = new SqlCommand("usp_KeyFactor_Insert", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@KeyFactorImage", (object?)fileUrl ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@KeyFactorName", request.KeyFactorName);
            cmd.Parameters.AddWithValue("@IsActive", request.IsActive);
            cmd.Parameters.AddWithValue("@InitiativeId", request.InitiativeId);

            var resultCode = new SqlParameter("@ResultCode", SqlDbType.Int)
            { Direction = ParameterDirection.Output };

            var resultMsg = new SqlParameter("@ResultMessage", SqlDbType.NVarChar, 500)
            { Direction = ParameterDirection.Output };

            cmd.Parameters.Add(resultCode);
            cmd.Parameters.Add(resultMsg);

            await cmd.ExecuteNonQueryAsync();

            return (
                Convert.ToInt32(resultCode.Value),
                resultMsg.Value?.ToString() ?? "Operation completed."
            );
        }

        // ============================================================
        //                        UPDATE KEY FACTOR
        // ============================================================
        public async Task<(int Code, string Message)> UpdateKeyFactorAsync(
            int keyFactorId,
            NewKeyFactorMasterRequest request,
            string? fileUrl)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var cmd = new SqlCommand("usp_KeyFactor_Update", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@KeyFactorId", keyFactorId);
            cmd.Parameters.AddWithValue("@KeyFactorImage", (object?)fileUrl ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@KeyFactorName", request.KeyFactorName);
            cmd.Parameters.AddWithValue("@IsActive", request.IsActive);
            cmd.Parameters.AddWithValue("@InitiativeId", request.InitiativeId);

            var resultCode = new SqlParameter("@ResultCode", SqlDbType.Int)
            { Direction = ParameterDirection.Output };

            var resultMsg = new SqlParameter("@ResultMessage", SqlDbType.NVarChar, 500)
            { Direction = ParameterDirection.Output };

            cmd.Parameters.Add(resultCode);
            cmd.Parameters.Add(resultMsg);

            await cmd.ExecuteNonQueryAsync();

            return (
                Convert.ToInt32(resultCode.Value),
                resultMsg.Value?.ToString() ?? "Operation completed."
            );
        }

        // ============================================================
        //                        GET ALL KEY FACTORS
        // ============================================================
        public async Task<List<NewKeyFactorMasterResponse>> GetKeyFactorsAsync(
            bool? isActive,
            int? initiativeId)
        {
            var list = new List<NewKeyFactorMasterResponse>();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var cmd = new SqlCommand("usp_KeyFactor_GetAll", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@IsActive", (object?)isActive ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@InitiativeId", (object?)initiativeId ?? DBNull.Value);

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new NewKeyFactorMasterResponse
                {
                    KeyFactorId = Convert.ToInt32(reader["KeyFactorId"]),
                    KeyFactorName = reader["KeyFactorName"]?.ToString(),
                    KeyFactorImage = reader["KeyFactorImage"]?.ToString(), // RELATIVE PATH ONLY
                    IsActive = Convert.ToBoolean(reader["IsActive"]),
                    InitiativeId = Convert.ToInt32(reader["InitiativeId"])
                });
            }

            return list;
        }

        // ============================================================
        //                     GET KEY FACTOR BY ID
        // ============================================================
        public async Task<NewKeyFactorMasterResponse?> GetKeyFactorByIdAsync(int keyFactorId)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var cmd = new SqlCommand("usp_KeyFactor_GetById", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@KeyFactorId", keyFactorId);

            using var reader = await cmd.ExecuteReaderAsync();
            if (!await reader.ReadAsync())
                return null;

            return new NewKeyFactorMasterResponse
            {
                KeyFactorId = Convert.ToInt32(reader["KeyFactorId"]),
                KeyFactorName = reader["KeyFactorName"]?.ToString(),
                KeyFactorImage = reader["KeyFactorImage"]?.ToString(), // RELATIVE PATH ONLY
                IsActive = Convert.ToBoolean(reader["IsActive"]),
                InitiativeId = Convert.ToInt32(reader["InitiativeId"])
            };
        }
    }
}
