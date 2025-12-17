using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using MSINS_API.Models.Request;
using MSINS_API.Models.Response;
using MSINS_API.POCO;
using MSINS_API.Repositories.Interface;
using System.Data;
using System.Net;

namespace MSINS_API.Repositories.Implementation
{
    public class NewInitiativeSpeakerRepository : INewInitiativeSpeakerRepository
    {
        private readonly string _connectionString;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly BaseUrlSettings _baseUrlSettings;

        public NewInitiativeSpeakerRepository(
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

        // -------------------------------------------------------------
        // 🔹 ADD SPEAKER
        // -------------------------------------------------------------
        public async Task<(int Code, string Message)> AddSpeakerAsync(NewInitiativeSpeakerRequest request, string fileUrl)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand("usp_NewInitiativeSpeaker_Add", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@Name", request.Name);
            command.Parameters.AddWithValue("@Designation", request.Designation);
            command.Parameters.AddWithValue("@ProfilePicUrl", fileUrl);   // ✔ FIXED
            command.Parameters.AddWithValue("@IsActive", request.IsActive);

            var resultCode = new SqlParameter("@ResultCode", SqlDbType.Int)
            { Direction = ParameterDirection.Output };

            var resultMessage = new SqlParameter("@ResultMessage", SqlDbType.NVarChar, 500)
            { Direction = ParameterDirection.Output };

            command.Parameters.Add(resultCode);
            command.Parameters.Add(resultMessage);

            await command.ExecuteNonQueryAsync();

            return (
                Convert.ToInt32(resultCode.Value),
                resultMessage.Value?.ToString() ?? "Operation completed."
            );
        }

        // -------------------------------------------------------------
        // 🔹 UPDATE SPEAKER
        // -------------------------------------------------------------
        public async Task<(int Code, string Message)> UpdateSpeakerAsync(int speakerId, NewInitiativeSpeakerRequest request, string fileUrl)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand("usp_NewInitiativeSpeaker_Update", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@SpeakerId", speakerId);
            command.Parameters.AddWithValue("@Name", request.Name);
            command.Parameters.AddWithValue("@Designation", request.Designation);
            command.Parameters.AddWithValue("@ProfilePicUrl", (object?)fileUrl ?? DBNull.Value); // ✔ FIXED
            command.Parameters.AddWithValue("@IsActive", request.IsActive);

            var resultCode = new SqlParameter("@ResultCode", SqlDbType.Int)
            { Direction = ParameterDirection.Output };

            var resultMessage = new SqlParameter("@ResultMessage", SqlDbType.NVarChar, 500)
            { Direction = ParameterDirection.Output };

            command.Parameters.Add(resultCode);
            command.Parameters.Add(resultMessage);

            await command.ExecuteNonQueryAsync();

            return (
                Convert.ToInt32(resultCode.Value),
                resultMessage.Value?.ToString() ?? "Operation completed."
            );
        }

        // -------------------------------------------------------------
        // 🔹 GET BY ID (FINAL WORKING VERSION WITHOUT AUTOMAPPER)
        // -------------------------------------------------------------
        public async Task<NewInitiativeSpeakerResponse?> GetSpeakerByIdAsync(int speakerId)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand("usp_NewInitiativeSpeaker_GetById", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@SpeakerId", speakerId);

            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                var item = new NewInitiativeSpeakerResponse
                {
                    SpeakerId = reader["SpeakerId"] != DBNull.Value ? Convert.ToInt32(reader["SpeakerId"]) : 0,
                    Name = reader["Name"]?.ToString(),
                    Designation = reader["Designation"]?.ToString(),
                    ProfilePicUrl = reader["ProfilePicUrl"]?.ToString(),
                    IsActive = reader["IsActive"] != DBNull.Value && Convert.ToBoolean(reader["IsActive"])
                };

                // Build absolute URL
                var request = _httpContextAccessor.HttpContext?.Request;
                string baseUrl = request != null
                    ? $"{request.Scheme}://{request.Host}"
                    : _baseUrlSettings.Production;

                if (!string.IsNullOrEmpty(item.ProfilePicUrl))
                {
                    item.ProfilePicUrl = $"{baseUrl}/{item.ProfilePicUrl.TrimStart('/')}";
                }

                return item;
            }

            return null;
        }


        // -------------------------------------------------------------
        // 🔹 GET ALL (FINAL WORKING VERSION WITHOUT AUTOMAPPER)
        // -------------------------------------------------------------
        public async Task<PagedResponse<NewInitiativeSpeakerResponse>> GetAllSpeakersAsync(string? name, string? designation, bool? isActive, int pageIndex, int pageSize)
        {
            var speakerList = new List<NewInitiativeSpeakerResponse>();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand("usp_NewInitiativeSpeaker_GetAll", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@Name", (object?)name ?? DBNull.Value);
            command.Parameters.AddWithValue("@Designation", (object?)designation ?? DBNull.Value);
            command.Parameters.AddWithValue("@IsActive", isActive.HasValue ? (object)isActive.Value : DBNull.Value);
            command.Parameters.AddWithValue("@PageIndex", pageIndex);
            command.Parameters.AddWithValue("@PageSize", pageSize);

            var totalRecordsParam = new SqlParameter("@TotalRecords", SqlDbType.Int)
            { Direction = ParameterDirection.Output };
            command.Parameters.Add(totalRecordsParam);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var record = new NewInitiativeSpeakerResponse
                {
                    SpeakerId = reader["SpeakerId"] != DBNull.Value ? Convert.ToInt32(reader["SpeakerId"]) : 0,
                    Name = reader["Name"]?.ToString(),
                    Designation = reader["Designation"]?.ToString(),
                    ProfilePicUrl = reader["ProfilePicUrl"]?.ToString(),
                    IsActive = reader["IsActive"] != DBNull.Value && Convert.ToBoolean(reader["IsActive"])
                };

                // Build absolute URL
                var request = _httpContextAccessor.HttpContext?.Request;
                string baseUrl = request != null
                    ? $"{request.Scheme}://{request.Host}"
                    : _baseUrlSettings.Production;

                if (!string.IsNullOrEmpty(record.ProfilePicUrl))
                {
                    record.ProfilePicUrl = $"{baseUrl}/{record.ProfilePicUrl.TrimStart('/')}";
                }

                speakerList.Add(record);
            }

            int totalRecords = totalRecordsParam.Value != DBNull.Value
                ? Convert.ToInt32(totalRecordsParam.Value)
                : 0;

            return new PagedResponse<NewInitiativeSpeakerResponse>
            {
                Data = speakerList,
                TotalRecords = totalRecords
            };




        }
    }
}
