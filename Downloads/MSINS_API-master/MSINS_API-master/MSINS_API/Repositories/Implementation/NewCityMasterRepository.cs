using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using MSINS_API.Models.Request;
using MSINS_API.Models.Response;
using MSINS_API.POCO;
using MSINS_API.Repositories.Interface;
using System.Data;
using MSINS_API.Models.Request;
using MSINS_API.Controllers;


namespace MSINS_API.Repositories.Implementation
{
    public class NewCityMasterRepository : INewCityMasterRepository
    {
        private readonly string _connectionString;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly BaseUrlSettings _baseUrlSettings;

        public NewCityMasterRepository(IHttpContextAccessor httpContextAccessor, IOptions<BaseUrlSettings> baseUrlSettings, IConfiguration configuration, IMapper mapper)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _baseUrlSettings = baseUrlSettings.Value;
        }

        //---------------------------- ADD CITY-----------------------------------------------------

        // -------------------------------UPDATE CITY-----------------------------------------------
        public async Task<(int Code, string Message)> UpdateCityAsync(int cityId, NewCityMasterRequest dto)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("usp_NewCity_Update", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@CityId", cityId);
                    command.Parameters.AddWithValue("@CityName", dto.CityName);
                    command.Parameters.AddWithValue("@IsActive", dto.IsActive);
                    command.Parameters.AddWithValue("@UserId", dto.adminId);

                    var resultCodeParam = new SqlParameter("@ResultCode", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    var resultMessageParam = new SqlParameter("@ResultMessage", SqlDbType.NVarChar, 500) { Direction = ParameterDirection.Output };

                    command.Parameters.Add(resultCodeParam);
                    command.Parameters.Add(resultMessageParam);

                    await command.ExecuteNonQueryAsync();

                    int resultCode = resultCodeParam.Value != DBNull.Value ? Convert.ToInt32(resultCodeParam.Value) : 0;
                    string resultMessage = resultMessageParam.Value?.ToString() ?? "Operation completed.";

                    return (resultCode, resultMessage);
                }
            }
        }


        // ------------------------------------------------- GET CITY BY ID -----------------------------------------------
        public async Task<NewCityMasterResponse?> GetCityByIdAsync(int cityId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("usp_NewCity_GetById", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CityId", cityId);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new NewCityMasterResponse
                            {
                                CityId = reader["CityId"] != DBNull.Value
                                    ? Convert.ToInt32(reader["CityId"])
                                    : 0,

                                CityName = reader["CityName"] != DBNull.Value
                                    ? reader["CityName"].ToString()
                                    : string.Empty,

                                IsActive = reader["IsActive"] != DBNull.Value
                                    ? Convert.ToBoolean(reader["IsActive"])
                                    : false,

                                adminId = reader["adminId"] != DBNull.Value
                                    ? Convert.ToInt32(reader["adminId"])
                                    : 0
                            };
                        }
                    }
                }
            }

            return null;
        }

        // ------------------------------------------------- GET CITY LIST -----------------------------------------------
        public async Task<PagedResponse<NewCityMasterResponse>> GetCityListAsync(bool? isActive)
        {
            var list = new List<NewCityMasterResponse>();
            int totalRecords = 0;

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("usp_NewCity_GetAll_OnlyIsActive", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@IsActive", isActive.HasValue ? (object)isActive.Value : DBNull.Value);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            list.Add(new NewCityMasterResponse
                            {
                                CityId = Convert.ToInt32(reader["CityId"]),
                                CityName = reader["CityName"].ToString(),
                                IsActive = Convert.ToBoolean(reader["IsActive"])
                            });
                        }
                    }
               }
            }

            return new PagedResponse<NewCityMasterResponse>
            {
                Data = list,
                TotalRecords = list.Count
            };
        }

    }
}