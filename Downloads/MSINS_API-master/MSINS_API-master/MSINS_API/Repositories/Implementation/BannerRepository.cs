using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using MSINS_API.Models.Request;
using MSINS_API.Models.Response;
using MSINS_API.POCO;
using MSINS_API.Repositories.Interface;
using System.Data;

namespace MSINS_API.Repositories.Implementation
{
    public class BannerRepository : IBannerRepository
    {
        private readonly string _connectionString;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly BaseUrlSettings _baseUrlSettings;


        public BannerRepository(IHttpContextAccessor httpContextAccessor, IOptions<BaseUrlSettings> baseUrlSettings, IConfiguration configuration, IMapper mapper)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _baseUrlSettings = baseUrlSettings.Value;
        }

        public async Task<(int Code, string Message)> AddOrUpdateBannerAsync(BannerRequest bannerDto, string? fileUrl)
        {
            int statusCode = 500;
            string message = "Unknown error occurred.";

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("AddOrUpdateBanner", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@BannerId", bannerDto.BannerId);
                    command.Parameters.AddWithValue("@BannerType", bannerDto.BannerType);
                    command.Parameters.AddWithValue("@BannerName", bannerDto.BannerName);
                    command.Parameters.AddWithValue("@BannerLink", (object?)bannerDto.BannerLink ?? DBNull.Value);
                    command.Parameters.AddWithValue("@LinkType", (object?)bannerDto.LinkType ?? DBNull.Value);
                    command.Parameters.AddWithValue("@ImagePath", (object?)fileUrl ?? DBNull.Value);
                    command.Parameters.AddWithValue("@IsActive", bannerDto.IsActive);
                    command.Parameters.AddWithValue("@UserId", bannerDto.adminId);

                    // Output parameters for Status Code and Message
                    var statusParam = new SqlParameter("@ResultCode", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    var messageParam = new SqlParameter("@ResultMessage", SqlDbType.NVarChar, 500)
                    {
                        Direction = ParameterDirection.Output
                    };

                    command.Parameters.Add(statusParam);
                    command.Parameters.Add(messageParam);

                    await command.ExecuteNonQueryAsync();

                    // Retrieve output values
                    statusCode = Convert.ToInt32(statusParam.Value);
                    message = messageParam.Value.ToString() ?? "No message returned.";

                    return (statusCode, message);
                }
            }
        }

        public async Task<PagedResponse<BannerResponse>> GetBannersAsync(BannerQueryParamsRequest queryParams)
        {
            var banners = new List<BannerResponse>();
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("GetAllBanners", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@bannerType", (object?)queryParams.BannerType ?? DBNull.Value);
                    command.Parameters.AddWithValue("@bannerName", (object?)queryParams.BannerName ?? DBNull.Value);
                    command.Parameters.AddWithValue("@isActive", queryParams.IsActive.HasValue ? (object)queryParams.IsActive.Value : DBNull.Value);
                    command.Parameters.AddWithValue("@pageIndex", queryParams.PageIndex);
                    command.Parameters.AddWithValue("@pageSize", queryParams.PageSize);

                    var totalRecordsParam = new SqlParameter("@totalRecords", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(totalRecordsParam);

                    
                    using (var reader = await command.ExecuteReaderAsync())
                    {

                        while (await reader.ReadAsync())
                        {
                            // Map the current reader row to the RecruiterListEFModel using AutoMapper
                            var recordDetail = _mapper.Map<BannerResponse>(reader);

                            // Get Base URL dynamically from HttpContext
                            var request = _httpContextAccessor.HttpContext?.Request;
                            string baseUrl = request != null
                                ? $"{request.Scheme}://{request.Host}"
                                : _baseUrlSettings.Production;       // Production URL from settings


                            if (!string.IsNullOrEmpty(recordDetail.ImagePath))
                            {
                                recordDetail.ImagePath = $"{baseUrl}/{recordDetail.ImagePath.TrimStart('/')}";
                            }

                            banners.Add(recordDetail); // Add the mapped recruiter to the list


                        }

                    }
                    int totalRecords = totalRecordsParam.Value != DBNull.Value ? Convert.ToInt32(totalRecordsParam.Value) : 0;
                    return new PagedResponse<BannerResponse>
                    {
                        Data = banners,
                        TotalRecords = totalRecords
                    };
                }
            }
        }
    }
}
