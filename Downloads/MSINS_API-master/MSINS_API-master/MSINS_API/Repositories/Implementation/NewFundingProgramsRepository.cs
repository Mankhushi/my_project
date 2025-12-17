using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using MSINS_API.Models.Request;
using MSINS_API.Models.Response;
using MSINS_API.POCO;
using MSINS_API.Repositories.Interface;
using System.Data;

namespace MSINS_API.Repositories.Implementation
{
    public class NewFundingProgramsRepository : INewFundingProgramsRepository
    {
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly BaseUrlSettings _baseUrlSettings;
        private readonly IWebHostEnvironment _env;

        public NewFundingProgramsRepository(
            IHttpContextAccessor httpContextAccessor,
            IOptions<BaseUrlSettings> baseUrlSettings,
            IConfiguration configuration,
            IWebHostEnvironment env)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _httpContextAccessor = httpContextAccessor;
            _baseUrlSettings = baseUrlSettings.Value;
            _env = env;
        }

        // ============================================================
        //                  SAVE FILE AND RETURN PATH
        // ============================================================
        private async Task<string?> SaveLogoAsync(IFormFile? file)
        {
            if (file == null) return null;

            string rootPath = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            string folder = Path.Combine(rootPath, "FundingProgramLogos");

            Directory.CreateDirectory(folder);

            string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            string filePath = Path.Combine(folder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"FundingProgramLogos/{fileName}";
        }

        // ============================================================
        //                  ADD FUNDING PROGRAM
        // ============================================================
        public async Task<(int Code, string Message)> AddFundingProgramAsync(NewFundingProgramsRequest request)
        {
            string? logoPath = await SaveLogoAsync(request.Logo);

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("usp_FundingPrograms_Insert", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Logo", (object?)logoPath ?? DBNull.Value);
                    command.Parameters.AddWithValue("@FundingAgencyName", request.FundingAgencyName);
                    command.Parameters.AddWithValue("@WebsiteLink", (object?)request.WebsiteLink ?? DBNull.Value);
                    command.Parameters.AddWithValue("@IsActive", request.IsActive);

                    var idParam = new SqlParameter("@FundProgramId", SqlDbType.Int)
                    { Direction = ParameterDirection.Output };

                    var codeParam = new SqlParameter("@ResultCode", SqlDbType.Int)
                    { Direction = ParameterDirection.Output };

                    var msgParam = new SqlParameter("@ResultMessage", SqlDbType.NVarChar, 500)
                    { Direction = ParameterDirection.Output };

                    command.Parameters.Add(idParam);
                    command.Parameters.Add(codeParam);
                    command.Parameters.Add(msgParam);

                    await command.ExecuteNonQueryAsync();

                    int code = Convert.ToInt32(codeParam.Value);
                    string message = msgParam.Value?.ToString() ?? "Operation completed";

                    return (code, message);
                }
            }
        }

        // ============================================================
        //                  UPDATE FUNDING PROGRAM
        // ============================================================
        public async Task<(int Code, string Message)> UpdateFundingProgramAsync(int fundProgramId, NewFundingProgramsRequest request)
        {
            string? logoPath = await SaveLogoAsync(request.Logo);

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("usp_FundingPrograms_Update", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@FundProgramId", fundProgramId);
                    command.Parameters.AddWithValue("@Logo", (object?)logoPath ?? DBNull.Value);
                    command.Parameters.AddWithValue("@FundingAgencyName", request.FundingAgencyName);
                    command.Parameters.AddWithValue("@WebsiteLink", (object?)request.WebsiteLink ?? DBNull.Value);
                    command.Parameters.AddWithValue("@IsActive", request.IsActive);

                    var codeParam = new SqlParameter("@ResultCode", SqlDbType.Int)
                    { Direction = ParameterDirection.Output };

                    var msgParam = new SqlParameter("@ResultMessage", SqlDbType.NVarChar, 500)
                    { Direction = ParameterDirection.Output };

                    command.Parameters.Add(codeParam);
                    command.Parameters.Add(msgParam);

                    await command.ExecuteNonQueryAsync();

                    int code = Convert.ToInt32(codeParam.Value);
                    string message = msgParam.Value?.ToString() ?? "Operation completed";

                    return (code, message);
                }
            }
        }

        // ============================================================
        //          GET ALL FUNDING PROGRAMS (PAGINATION)
        // ============================================================
        public async Task<PagedResponse<NewFundingProgramsResponse>> GetFundingProgramsAsync(
     string? fundingAgencyName,
     bool? isActive,
     int pageIndex,
     int pageSize)
        {
            var results = new List<NewFundingProgramsResponse>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("usp_FundingPrograms_GetAll", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@FundingAgencyName", (object?)fundingAgencyName ?? DBNull.Value);
                    command.Parameters.AddWithValue("@IsActive", (object?)isActive ?? DBNull.Value);
                    command.Parameters.AddWithValue("@PageIndex", pageIndex);
                    command.Parameters.AddWithValue("@PageSize", pageSize);

                    var totalRecordsParam = new SqlParameter("@TotalRecords", SqlDbType.Int)
                    { Direction = ParameterDirection.Output };

                    command.Parameters.Add(totalRecordsParam);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var record = new NewFundingProgramsResponse
                            {
                                FundProgramId = Convert.ToInt32(reader["FundProgramId"]),
                                Logo = reader["Logo"]?.ToString(),
                                FundingAgencyName = reader["FundingAgencyName"]?.ToString(),
                                WebsiteLink = reader["WebsiteLink"]?.ToString(),
                                IsActive = Convert.ToBoolean(reader["IsActive"])
                            };

                            var req = _httpContextAccessor.HttpContext?.Request;

                            string baseUrl = req != null
                                ? $"{req.Scheme}://{req.Host}"
                                : _baseUrlSettings.Production;

                            if (!string.IsNullOrEmpty(record.Logo))
                                record.Logo = $"{baseUrl}/{record.Logo.TrimStart('/')}";

                            results.Add(record);
                        }
                    }

                    int totalRecords = totalRecordsParam.Value != DBNull.Value
                        ? Convert.ToInt32(totalRecordsParam.Value)
                        : 0;

                    return new PagedResponse<NewFundingProgramsResponse>
                    {
                        Data = results,
                        TotalRecords = totalRecords,
                        PageIndex = pageIndex,
                        PageSize = pageSize
                    };
                }
            }
        }

        // ============================================================
        //                GET FUNDING PROGRAM BY ID
        // ============================================================
        public async Task<NewFundingProgramsResponse?> GetFundingProgramByIdAsync(int fundProgramId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("usp_FundingPrograms_GetById", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@FundProgramId", fundProgramId);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            var record = new NewFundingProgramsResponse
                            {
                                FundProgramId = Convert.ToInt32(reader["FundProgramId"]),
                                Logo = reader["Logo"]?.ToString(),
                                FundingAgencyName = reader["FundingAgencyName"]?.ToString(),
                                WebsiteLink = reader["WebsiteLink"]?.ToString(),
                                IsActive = Convert.ToBoolean(reader["IsActive"])
                            };

                            var req = _httpContextAccessor.HttpContext?.Request;
                            string baseUrl = req != null
                                ? $"{req.Scheme}://{req.Host}"
                                : _baseUrlSettings.Production;

                            if (!string.IsNullOrEmpty(record.Logo))
                                record.Logo = $"{baseUrl}/{record.Logo.TrimStart('/')}";

                            return record;
                        }
                    }
                }
            }

            return null;
        }
    }
}
