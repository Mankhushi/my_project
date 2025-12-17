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
    public class NewSectorMasterRepository : INewSectorMasterRepository
    {
        private readonly string _connectionString;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly BaseUrlSettings _baseUrlSettings;

        public NewSectorMasterRepository(
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

        /// Adds a new sector -------------------------------------------------------------------->>>>>>>>>>>>>>>>>>
        public async Task<(int Code, string Message)> AddSectorAsync(NewSectorMasterRequest sectorRequest)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("usp_Sector_Insert", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@SectorName", sectorRequest.SectorName);
                    command.Parameters.AddWithValue("@IsActive", sectorRequest.IsActive);
                    command.Parameters.AddWithValue("@UserId", 1); // Replace with logged-in user if needed
                    command.Parameters.AddWithValue("@AdminId", 1);

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

                    int resultCode = resultCodeParam.Value != DBNull.Value ? Convert.ToInt32(resultCodeParam.Value) : 0;
                    string resultMessage = resultMessageParam.Value?.ToString() ?? "Operation completed.";

                    return (resultCode, resultMessage);
                }
            }
        }


        /// Updates an existing sector ----------------------------------------------------->>>>>>>>>
        public async Task<(int Code, string Message)> UpdateSectorAsync(NewSectorMasterRequest sectorRequest)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("usp_Sector_Update", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // ✅ Mandatory SectorId check (no throw)
                    if (sectorRequest.SectorId == null || sectorRequest.SectorId <= 0)
                        return (400, "SectorId is required for updating a sector.");

                    // ✅ Parameters (must match stored procedure exactly)
                    command.Parameters.AddWithValue("@SectorId", sectorRequest.SectorId);
                    command.Parameters.AddWithValue("@SectorName", (object?)sectorRequest.SectorName ?? DBNull.Value);
                    command.Parameters.AddWithValue("@IsActive", sectorRequest.IsActive);
                    command.Parameters.AddWithValue("@UserId", sectorRequest.AdminId);

                    // ✅ Output parameters
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

                    // ✅ Execute procedure
                    await command.ExecuteNonQueryAsync();

                    // ✅ Read output parameters
                    int resultCode = resultCodeParam.Value != DBNull.Value
                        ? Convert.ToInt32(resultCodeParam.Value)
                        : 0;

                    string resultMessage = resultMessageParam.Value?.ToString() ?? "Operation completed.";

                    // ✅ Return tuple
                    return (resultCode, resultMessage);
                }
            }
        }



        /// <summary>
        /// Retrieves a sector by its ID.
        /// </summary>
        public async Task<NewSectorMasterResponse?> GetSectorByIdAsync(int sectorId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("usp_Sector_GetById", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@SectorId", sectorId);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new NewSectorMasterResponse
                            {
                                SectorId = reader.GetInt32(reader.GetOrdinal("SectorId")),
                                SectorName = reader.GetString(reader.GetOrdinal("SectorName")),
                                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
                            };
                        }
                    }
                }
            }

            return null; // No record found
        }


        // Get all ------>>>>
        public async Task<List<NewSectorMasterResponse>> GetSectorsForWebsiteAsync(bool? isActive)
        {
            var list = new List<NewSectorMasterResponse>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("dbo.usp_NewSector_GetAll_OnlyIsActive", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@IsActive", isActive.HasValue ? (object)isActive.Value : DBNull.Value);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            list.Add(new NewSectorMasterResponse
                            {
                                SectorId = Convert.ToInt32(reader["SectorId"]),
                                SectorName = reader["SectorName"].ToString(),
                                IsActive = Convert.ToBoolean(reader["IsActive"])
                            });
                        }
                    }
                }
            }

            return list;
        }
    }
}
