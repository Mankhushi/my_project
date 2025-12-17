using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using MSINS_API.Models.Request;
using MSINS_API.Models.Response;
using MSINS_API.POCO;
using MSINS_API.Repositories.Interface;
using System.Data;

namespace MSINS_API.Repositories.Implementation
{
    public class NewSpeakersMasterRepository : INewSpeakersMasterRepository
    {
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly BaseUrlSettings _baseUrlSettings;

        public NewSpeakersMasterRepository(
            IHttpContextAccessor httpContextAccessor,
            IOptions<BaseUrlSettings> baseUrlSettings,
            IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _httpContextAccessor = httpContextAccessor;
            _baseUrlSettings = baseUrlSettings.Value;
        }

        // ADD SPEAKER  ----------------------------------------->>>>>>>>>>>>>
        public async Task<(int Code, string Message)> AddSpeakerAsync(NewSpeakersMasterRequest model, string? fileUrl)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("dbo.usp_Speaker_Add", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Input parameters
                    command.Parameters.AddWithValue("@SpeakerName", model.SpeakersName ?? (object)DBNull.Value);
                    
                    
                    command.Parameters.AddWithValue("@Designation", model.Designation ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Organization", model.Organization ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@ProfilePicPath", string.IsNullOrEmpty(fileUrl) ? DBNull.Value : fileUrl);
                    command.Parameters.AddWithValue("@IsActive", model.IsActive);
                    command.Parameters.AddWithValue("@UserId", model.adminId);
                    command.Parameters.AddWithValue("@InitiativeId", model.InitiativeId);

                    //  Output parameters
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
                    string resultMessage = resultMessageParam.Value != DBNull.Value
                        ? resultMessageParam.Value.ToString()
                        : "Speaker added successfully.";

                    return (resultCode, resultMessage);
                }
            }
        }

        //  ✅ UPDATE SPEAKER (Partial Update Supported)
        public async Task<(int Code, string Message)> UpdateSpeakerAsync(int id, NewSpeakersMasterRequest model, string? fileUrl)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // 🧩 Step 1: Fetch existing speaker data
                NewSpeakerMasterResponse? existingSpeaker = null;

                using (var getCommand = new SqlCommand("dbo.usp_Speaker_GetById", connection))
                {
                    getCommand.CommandType = CommandType.StoredProcedure;
                    getCommand.Parameters.AddWithValue("@SpeakerId", id);

                    using (var reader = await getCommand.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            existingSpeaker = new NewSpeakerMasterResponse
                            {
                                SpeakerId = id,
                                SpeakerName = reader["SpeakerName"]?.ToString(),
                                Designation = reader["Designation"]?.ToString(),                        
                                Organization = reader["Organization"]?.ToString(),
                                ProfilePic = reader["ProfilePic"]?.ToString(),
                                IsActive = Convert.ToBoolean(reader["IsActive"]),
                                InitiativeId = reader["InitiativeId"] != DBNull.Value ? Convert.ToInt32(reader["InitiativeId"]) : 0,
              
                            };
                        }
                    }
                }

                if (existingSpeaker == null)
                    return (0, "Speaker not found.");

                // 🧩 Step 2: Delete old profile pic only if new one is uploaded
                if (!string.IsNullOrEmpty(fileUrl) && !string.IsNullOrEmpty(existingSpeaker.ProfilePic))
                {
                    var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), existingSpeaker.ProfilePic);
                    if (File.Exists(oldFilePath))
                    {
                        File.Delete(oldFilePath);
                    }
                }

                // 🧩 Step 3: Update only provided fields
                using (var command = new SqlCommand("dbo.usp_Speaker_Update", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@SpeakerId", id);
                    command.Parameters.AddWithValue("@SpeakerName", string.IsNullOrWhiteSpace(model.SpeakersName) ? DBNull.Value : model.SpeakersName);
                    command.Parameters.AddWithValue("@Designation", string.IsNullOrWhiteSpace(model.Designation) ? DBNull.Value : model.Designation);
                    command.Parameters.AddWithValue("@Organization",string.IsNullOrWhiteSpace(model.Organization) ? DBNull.Value : model.Organization);
                    command.Parameters.AddWithValue("@ProfilePicPath", string.IsNullOrEmpty(fileUrl) ? DBNull.Value : fileUrl);
                    command.Parameters.AddWithValue("@InitiativeId", model.InitiativeId);



                    //command.Parameters.AddWithValue("@UserId", model.adminId);

                    // ✅ Proper null-safe IsActive handling
                    command.Parameters.AddWithValue("@IsActive", model.IsActive);

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

                    int resultCode = resultCodeParam.Value != DBNull.Value
                        ? Convert.ToInt32(resultCodeParam.Value)
                        : 0;
                    string resultMessage = resultMessageParam.Value?.ToString() ?? "Speaker updated successfully.";

                    return (resultCode, resultMessage);
                }
            }
        }


        // GET SPEAKER BY ID  ------------------------------>>>>
        public async Task<NewSpeakerMasterResponse?> GetSpeakerByIdAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("dbo.usp_Speaker_GetById", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@SpeakerId", id);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            // Build full image URL dynamically
                            var request = _httpContextAccessor.HttpContext?.Request;
                            string baseUrl = request != null
                                ? $"{request.Scheme}://{request.Host}"
                                : _baseUrlSettings.Production;

                            string? imagePath = reader["ProfilePic"] != DBNull.Value
                                ? $"{baseUrl}/{reader["ProfilePic"].ToString()?.TrimStart('/')}"
                                : null;

                            return new NewSpeakerMasterResponse
                            {
                                SpeakerId = Convert.ToInt32(reader["SpeakerId"]),
                                SpeakerName = reader["SpeakerName"].ToString(),
                                Designation = reader["Designation"].ToString(),
                                Organization = reader["Organization"].ToString(),
                                InitiativeId = reader["InitiativeId"] != DBNull.Value ? Convert.ToInt32(reader["InitiativeId"]) : 0,

                                ProfilePic = imagePath,
                                IsActive = Convert.ToBoolean(reader["IsActive"])
                            };
                        }
                    }
                }
            }

            return null;
        }

        // GET ALL SPEAKERS (Paginated)  -------------------------------------------->>>
        public async Task<PagedResponse<NewSpeakerMasterResponse>> GetAllSpeakersAsync(int pageNumber, int pageSize, string? searchTerm = null)
        {
            var speakers = new List<NewSpeakerMasterResponse>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("dbo.usp_Speaker_GetAll", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@PageNumber", pageNumber);
                    command.Parameters.AddWithValue("@PageSize", pageSize);
                    command.Parameters.AddWithValue("@SearchTerm", (object?)searchTerm ?? DBNull.Value);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        // ✅ 1️⃣ First Result Set → Speaker Data
                        while (await reader.ReadAsync())
                        {
                            var request = _httpContextAccessor.HttpContext?.Request;
                            string baseUrl = request != null
                                ? $"{request.Scheme}://{request.Host}"
                                : _baseUrlSettings.Production;

                            string? imagePath = reader["ProfilePic"] != DBNull.Value
                                ? $"{baseUrl}/{reader["ProfilePic"].ToString()?.TrimStart('/')}"
                                : null;

                            speakers.Add(new NewSpeakerMasterResponse
                            {
                                SpeakerId = Convert.ToInt32(reader["SpeakerId"]),
                                SpeakerName = reader["SpeakerName"]?.ToString(),
                                Designation = reader["Designation"]?.ToString(),
                                Organization = reader["Organization"]?.ToString(),
                                InitiativeId = reader["InitiativeId"] != DBNull.Value
    ? Convert.ToInt32(reader["InitiativeId"])
    : 0,
                                ProfilePic = imagePath,
                                IsActive = Convert.ToBoolean(reader["IsActive"])
                            });
                        }

                        // ✅ 2️⃣ Second Result Set → Pagination Info
                        int totalRecords = 0;
                        int totalPages = 0;

                        if (await reader.NextResultAsync() && await reader.ReadAsync())
                        {
                            totalRecords = Convert.ToInt32(reader["TotalCount"]);
                            totalPages = Convert.ToInt32(reader["TotalPages"]);
                        }

                        return new PagedResponse<NewSpeakerMasterResponse>
                        {
                            Data = speakers,
                            TotalRecords = totalRecords,
                            TotalPages = totalPages,
                            PageNumber = pageNumber,
                            PageSize = pageSize,
                            Message = "Speakers fetched successfully."
                        };
                    }
                }
            }
        }
    }
}


