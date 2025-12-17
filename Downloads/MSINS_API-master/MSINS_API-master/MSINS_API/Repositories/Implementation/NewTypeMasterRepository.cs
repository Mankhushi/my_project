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
    public class NewTypeMasterRepository : INewTypeMasterRepository
    {
        private readonly string _connectionString;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly BaseUrlSettings _baseUrlSettings;

        public NewTypeMasterRepository(
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

        // 🔵 ADD TYPE MASTER
        public async Task<(bool IsSuccess, string Message, int TypeId)> AddTypeAsync(NewTypeMasterRequest request)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("usp_TypeMaster_Insert", conn);

            cmd.CommandType = CommandType.StoredProcedure;

            // ❌ REMOVE @p_TypeId FIXED
            cmd.Parameters.AddWithValue("@p_TypeName", request.TypeName);
            cmd.Parameters.AddWithValue("@p_SectorId", request.SectorId);
            cmd.Parameters.AddWithValue("@p_IsActive", request.IsActive);
            cmd.Parameters.AddWithValue("@p_AdminId", request.AdminId);

            SqlParameter outputIdParam = new SqlParameter("@p_NewTypeId", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(outputIdParam);

            await conn.OpenAsync();

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return (
                    reader.GetBoolean(reader.GetOrdinal("IsSuccess")),
                    reader.GetString(reader.GetOrdinal("Message")),
                    reader.GetInt32(reader.GetOrdinal("TypeId"))
                );
            }

            return (false, "Unexpected response from DB.", 0);
        }

        // 🟠 UPDATE TYPE MASTER
        public async Task<(bool IsSuccess, string Message)> UpdateTypeAsync(NewTypeMasterRequest request)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("usp_TypeMaster_Update", conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@p_TypeId", request.TypeId);
            cmd.Parameters.AddWithValue("@p_TypeName", request.TypeName);
            cmd.Parameters.AddWithValue("@p_SectorId", request.SectorId);
            cmd.Parameters.AddWithValue("@p_IsActive", request.IsActive);
            cmd.Parameters.AddWithValue("@p_AdminId", request.AdminId);

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return (
                    reader.GetBoolean(reader.GetOrdinal("IsSuccess")),
                    reader.GetString(reader.GetOrdinal("Message"))
                );
            }

            return (false, "Unexpected DB response.");
        }

        // 🟣 GET BY ID
        public async Task<NewTypeMasterResponse?> GetTypeByIdAsync(int typeId)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("usp_TypeMaster_GetById", conn);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@p_TypeId", typeId);

            await conn.OpenAsync();

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new NewTypeMasterResponse
                {
                    TypeId = reader.GetInt32(reader.GetOrdinal("TypeId")),
                    TypeName = reader.GetString(reader.GetOrdinal("TypeName")),
                    IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                    SectorName = reader.GetString(reader.GetOrdinal("SectorName")) // ⭐ NEW
                };
            }

            return null;
        }


        // 🟢 GET ALL (PAGED)
        public async Task<PagedResponse<NewTypeMasterResponse>> GetAllTypesPagedAsync(int pageNumber, int pageSize, string? searchTerm)
        {
            var list = new List<NewTypeMasterResponse>();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand("usp_TypeMaster_GetAll_Paged", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@PageNumber", pageNumber);
            command.Parameters.AddWithValue("@PageSize", pageSize);
            command.Parameters.AddWithValue("@SearchTerm", (object?)searchTerm ?? DBNull.Value);

            var totalRecordsParam = new SqlParameter("@TotalRecords", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            command.Parameters.Add(totalRecordsParam);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new NewTypeMasterResponse
                {
                    TypeId = Convert.ToInt32(reader["TypeId"]),
                    TypeName = reader["TypeName"].ToString(),
                    SectorName = reader["SectorName"].ToString(),   // ⭐ Required Output
                    IsActive = Convert.ToBoolean(reader["IsActive"])
                });
            }

            int totalRecords = Convert.ToInt32(totalRecordsParam.Value);

            return new PagedResponse<NewTypeMasterResponse>
            {
                Data = list,
                TotalRecords = totalRecords,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

    }
}
