using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using MSINS_API.Models.Request;
using MSINS_API.Models.Response;
using MSINS_API.POCO;
using MSINS_API.Repositories.Interface;
using System.Data;

namespace MSINS_API.Repositories.Implementation
{
    public class NewWinnerMasterRepository : INewWinnerMasterRepository
    {
        private readonly string _connectionString;

        public NewWinnerMasterRepository(
            IOptions<BaseUrlSettings> baseUrlSettings,
            IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // ============================================================
        //                         ADD WINNER
        // ============================================================
        public async Task<(int Code, string Message)> AddWinnerAsync(
            NewWinnerMasterRequest request,
            string? fileUrl)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var cmd = new SqlCommand("usp_Winner_Insert", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@SectorId", request.SectorId);
            cmd.Parameters.AddWithValue("@WinnerImage", fileUrl ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@WinnerName", request.WinnerName);
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
        //                         UPDATE WINNER
        // ============================================================
        public async Task<(int Code, string Message)> UpdateWinnerAsync(
            int winnerId,
            NewWinnerMasterRequest request,
            string? fileUrl)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var cmd = new SqlCommand("usp_Winner_Update", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@WinnerId", winnerId);
            cmd.Parameters.AddWithValue("@SectorId", request.SectorId);
            cmd.Parameters.AddWithValue("@WinnerImage", fileUrl ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@WinnerName", request.WinnerName);
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
        //                         GET ALL WINNERS
        // ============================================================
        public async Task<List<NewWinnerMasterResponse>> GetWinnersAsync(
            int? sectorId,
            bool? isActive,
            int? initiativeId)
        {
            var list = new List<NewWinnerMasterResponse>();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var cmd = new SqlCommand("usp_Winner_GetAll", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@SectorId", (object?)sectorId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@IsActive", (object?)isActive ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@InitiativeId", (object?)initiativeId ?? DBNull.Value);

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new NewWinnerMasterResponse
                {
                    WinnerId = Convert.ToInt32(reader["WinnerId"]),
                    SectorId = Convert.ToInt32(reader["SectorId"]),
                    WinnerImage = reader["WinnerImage"]?.ToString(), // ONLY RELATIVE PATH
                    WinnerName = reader["WinnerName"].ToString(),
                    IsActive = Convert.ToBoolean(reader["IsActive"]),
                    InitiativeId = Convert.ToInt32(reader["InitiativeId"])
                });
            }

            return list;
        }

        // ============================================================
        //                        GET WINNER BY ID
        // ============================================================
        public async Task<NewWinnerMasterResponse?> GetWinnerByIdAsync(int winnerId)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var cmd = new SqlCommand("usp_Winner_GetById", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@WinnerId", winnerId);

            using var reader = await cmd.ExecuteReaderAsync();
            if (!await reader.ReadAsync())
                return null;

            return new NewWinnerMasterResponse
            {
                WinnerId = Convert.ToInt32(reader["WinnerId"]),
                SectorId = Convert.ToInt32(reader["SectorId"]),
                WinnerImage = reader["WinnerImage"]?.ToString(), // ONLY RELATIVE PATH
                WinnerName = reader["WinnerName"].ToString(),
                IsActive = Convert.ToBoolean(reader["IsActive"]),
                InitiativeId = Convert.ToInt32(reader["InitiativeId"])
            };
        }
    }
}
