using Microsoft.Data.SqlClient;
using MSINS_API.Models.Request;
using MSINS_API.Models.Response;
using MSINS_API.Repositories.Interface;
using System.Data;

namespace MSINS_API.Repositories.Implementation
{
    public class NewPhotographsMasterRepository : INewPhotographsMasterRepository
    {
        private readonly string _connectionString;

        public NewPhotographsMasterRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // ============================================================
        //                    ADD PHOTOGRAPH
        // ============================================================
        public async Task<(int Code, string Message)> AddPhotographAsync(
            NewPhotographsMasterRequest request,
            string? fileUrl)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var cmd = new SqlCommand("usp_Photographs_Insert", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@PhotographName", request.PhotographName);
            cmd.Parameters.AddWithValue("@PhotographPath", (object?)fileUrl ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@InitiativeId", request.InitiativeId);
            cmd.Parameters.AddWithValue("@IsActive", request.IsActive);
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
        //                    UPDATE PHOTOGRAPH
        // ============================================================
        public async Task<(int Code, string Message)> UpdatePhotographAsync(
            int photographId,
            NewPhotographsMasterRequest request,
            string? fileUrl)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var cmd = new SqlCommand("usp_Photographs_Update", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@PhotographsId", photographId);
            cmd.Parameters.AddWithValue("@PhotographName", request.PhotographName);
            cmd.Parameters.AddWithValue("@PhotographPath", (object?)fileUrl ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@InitiativeId", request.InitiativeId);
            cmd.Parameters.AddWithValue("@IsActive", request.IsActive);
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
        //                    GET ALL PHOTOGRAPHS
        // ============================================================
        public async Task<List<NewPhotographsMasterResponse>> GetPhotographsAsync(
            int? initiativeId,
            bool? isActive)
        {
            var list = new List<NewPhotographsMasterResponse>();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var cmd = new SqlCommand("usp_Photographs_GetAll", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@InitiativeId", (object?)initiativeId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@IsActive", (object?)isActive ?? DBNull.Value);

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new NewPhotographsMasterResponse
                {
                    PhotographsId = Convert.ToInt32(reader["PhotographsId"]),
                    PhotographName = reader["PhotographName"]?.ToString(),
                    PhotographPath = reader["PhotographPath"]?.ToString(),   // RELATIVE PATH
                    InitiativeId = Convert.ToInt32(reader["InitiativeId"]),
                    IsActive = Convert.ToBoolean(reader["IsActive"])
                });
            }

            return list;
        }

        // ============================================================
        //                    GET PHOTOGRAPH BY ID
        // ============================================================
        public async Task<NewPhotographsMasterResponse?> GetPhotographByIdAsync(int photographId)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var cmd = new SqlCommand("usp_Photographs_GetById", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PhotographsId", photographId);

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new NewPhotographsMasterResponse
                {
                    PhotographsId = Convert.ToInt32(reader["PhotographsId"]),
                    PhotographName = reader["PhotographName"]?.ToString(),
                    PhotographPath = reader["PhotographPath"]?.ToString(),  // RELATIVE PATH
                    InitiativeId = Convert.ToInt32(reader["InitiativeId"]),
                    IsActive = Convert.ToBoolean(reader["IsActive"])
                };
            }

            return null;
        }
    }
}
