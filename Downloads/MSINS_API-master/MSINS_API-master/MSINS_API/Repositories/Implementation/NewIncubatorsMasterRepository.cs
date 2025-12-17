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
    public class NewIncubatorsMasterRepository : INewIncubatorsMasterRepository
    {
        private readonly string _connectionString;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly BaseUrlSettings _baseUrlSettings;

        public NewIncubatorsMasterRepository(
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

        //----------------Add-------------------------------------------
        public async Task<(bool IsSuccess, string Message, int IncubatorId)> AddAsync(NewIncubatorsMasterRequest request, string? logoPath)
        {
            using SqlConnection conn = new(_connectionString);
            using SqlCommand cmd = new("usp_NewIncubator_Add", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.Add("@IncubatorName", SqlDbType.NVarChar).Value = request.IncubatorName;
            cmd.Parameters.Add("@CityId", SqlDbType.Int).Value = request.CityId;
            cmd.Parameters.Add("@SectorId", SqlDbType.Int).Value = request.SectorId;
            cmd.Parameters.Add("@TypeId", SqlDbType.Int).Value = request.TypeId;
            cmd.Parameters.Add("@AdminId", SqlDbType.Int).Value = request.AdminId;
            cmd.Parameters.Add("@LogoPath", SqlDbType.NVarChar).Value = (object?)logoPath ?? DBNull.Value;
            cmd.Parameters.Add("@IsActive", SqlDbType.Bit).Value = request.IsActive;

            // 🔥 REQUIRED OUTPUT PARAMETERS
            var resultCodeParam = new SqlParameter("@ResultCode", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(resultCodeParam);

            var resultMessageParam = new SqlParameter("@ResultMessage", SqlDbType.NVarChar, 500)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(resultMessageParam);

            await conn.OpenAsync();
            var dt = new DataTable();
            dt.Load(await cmd.ExecuteReaderAsync());

            int resultCode = (int)resultCodeParam.Value;
            string resultMessage = resultMessageParam.Value?.ToString() ?? "No message";

            int incubatorId = 0;
            if (dt.Rows.Count > 0 && dt.Columns.Contains("IncubatorId"))
            {
                incubatorId = Convert.ToInt32(dt.Rows[0]["IncubatorId"]);
            }

            return (resultCode == 200, resultMessage, incubatorId);
        }

        //----------------Update-------------------------------------------
        public async Task<(bool IsSuccess, string Message, int IncubatorId)> UpdateAsync(
         NewIncubatorsMasterRequest request, string? logoPath)
        {
            using SqlConnection conn = new(_connectionString);
            using SqlCommand cmd = new("usp_NewIncubator_Update", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.Add("@IncubatorId", SqlDbType.Int).Value = request.IncubatorId ?? 0;
            cmd.Parameters.Add("@IncubatorName", SqlDbType.NVarChar).Value = request.IncubatorName;
            cmd.Parameters.Add("@CityId", SqlDbType.Int).Value = request.CityId;
            cmd.Parameters.Add("@SectorId", SqlDbType.Int).Value = request.SectorId;
            cmd.Parameters.Add("@TypeId", SqlDbType.Int).Value = request.TypeId;
            cmd.Parameters.Add("@AdminId", SqlDbType.Int).Value = request.AdminId;
            cmd.Parameters.Add("@LogoPath", SqlDbType.NVarChar).Value = (object?)logoPath ?? DBNull.Value;
            cmd.Parameters.Add("@IsActive", SqlDbType.Bit).Value = request.IsActive;

            var resultCodeParam = new SqlParameter("@ResultCode", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(resultCodeParam);

            var resultMessageParam = new SqlParameter("@ResultMessage", SqlDbType.NVarChar, 500)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(resultMessageParam);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();

            // ⭐ SAFE CONVERSION (FIX FOR YOUR ERROR)
            int resultCode = resultCodeParam.Value == DBNull.Value
                ? 500
                : Convert.ToInt32(resultCodeParam.Value);

            string resultMessage = resultMessageParam.Value == DBNull.Value
                ? "Unknown error"
                : resultMessageParam.Value.ToString()!;

            return (resultCode == 200, resultMessage, request.IncubatorId ?? 0);
        }


        //----------------Get By Id-------------------------------------------
        public async Task<NewIncubatorsMasterResponse?> GetByIdAsync(int incubatorId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("usp_NewIncubator_GetById", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@IncubatorId", incubatorId);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new NewIncubatorsMasterResponse
                            {
                                IncubatorId = Convert.ToInt32(reader["IncubatorId"]),
                                IncubatorName = reader["IncubatorName"].ToString(),
                                CityName = reader["CityName"].ToString(),
                                SectorName = reader["SectorName"].ToString(),
                                TypeName = reader["TypeName"].ToString(),
                                LogoPath = reader["LogoPath"].ToString(),
                                IsActive = Convert.ToBoolean(reader["IsActive"])
                            };
                        }
                    }
                }
            }

            return null;
        }


        //----------------Get All Without Pagination-------------------------------------------
        public async Task<List<NewIncubatorsMasterResponse>> GetAllWithoutPaginationAsync(
     string? citySearch,
     string? sectorSearch,
     string? typeSearch,
     bool? isActive)
        {
            var list = new List<NewIncubatorsMasterResponse>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("usp_GetIncubatorsWithoutPagination", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@CitySearch",
                        string.IsNullOrEmpty(citySearch) ? DBNull.Value : citySearch);

                    command.Parameters.AddWithValue("@SectorSearch",
                        string.IsNullOrEmpty(sectorSearch) ? DBNull.Value : sectorSearch);

                    command.Parameters.AddWithValue("@TypeSearch",
                        string.IsNullOrEmpty(typeSearch) ? DBNull.Value : typeSearch);

                    command.Parameters.AddWithValue("@IsActive",
                        isActive.HasValue ? (object)isActive.Value : DBNull.Value);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            list.Add(new NewIncubatorsMasterResponse
                            {
                                IncubatorId = Convert.ToInt32(reader["IncubatorId"]),
                                IncubatorName = reader["IncubatorName"].ToString(),
                                CityName = reader["CityName"].ToString(),
                                SectorName = reader["SectorName"].ToString(),
                                TypeName = reader["TypeName"].ToString(),
                                LogoPath = reader["LogoPath"].ToString(),
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

