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
    public class EventRepository : IEventRepository
    {
        private readonly string _connectionString;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly BaseUrlSettings _baseUrlSettings;


        public EventRepository(IHttpContextAccessor httpContextAccessor, IOptions<BaseUrlSettings> baseUrlSettings, IConfiguration configuration, IMapper mapper)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _baseUrlSettings = baseUrlSettings.Value;
        }

        public async Task<(int Code, string Message)> AddOrUpdateEventAsync(EventRequest EventDto, string? fileUrl)
        {
            int statusCode = 500;
            string message = "Unknown error occurred.";

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("AddOrUpdateEvent", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@EventId", EventDto.EventId);
                    command.Parameters.AddWithValue("@EventType", EventDto.EventType);
                    command.Parameters.AddWithValue("@EventDate", EventDto.EventDate);
                    command.Parameters.AddWithValue("@EventName", EventDto.EventName);
                    command.Parameters.AddWithValue("@EventLocation", EventDto.EventLocation);
                    command.Parameters.AddWithValue("@EventDesc", EventDto.EventDesc);
                    command.Parameters.AddWithValue("@ImagePath", (object?)fileUrl ?? DBNull.Value);
                    command.Parameters.AddWithValue("@IsActive", EventDto.IsActive);
                    command.Parameters.AddWithValue("@UserId", EventDto.AdminId);

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

        public async Task<PagedResponse<EventResponse>> GetEventsAsync(EventQueryParamRequest queryParams)
        {
            var events = new List<EventResponse>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("GetAllEvents", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Search", (object?)queryParams.Search ?? DBNull.Value);
                    command.Parameters.AddWithValue("@EventType", (object?)queryParams.EventType ?? DBNull.Value);
                    command.Parameters.AddWithValue("@IsActive", queryParams.IsActive.HasValue ? (object)queryParams.IsActive.Value : DBNull.Value);
                    command.Parameters.AddWithValue("@StartDate", (object?)queryParams.EventStartDate ?? DBNull.Value);
                    command.Parameters.AddWithValue("@EndDate", (object?)queryParams.EventEndDate ?? DBNull.Value);
                    command.Parameters.AddWithValue("@PageIndex", queryParams.PageIndex);
                    command.Parameters.AddWithValue("@PageSize", queryParams.PageSize);

                    var totalRecordsParam = new SqlParameter("@TotalRecords", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(totalRecordsParam);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var recordDetail = _mapper.Map<EventResponse>(reader);

                            // Get Base URL dynamically from HttpContext
                            var request = _httpContextAccessor.HttpContext?.Request;
                            string baseUrl = request != null
                                ? $"{request.Scheme}://{request.Host}"
                                : _baseUrlSettings.Production; // Production URL from settings

                            if (!string.IsNullOrEmpty(recordDetail.ImagePath))
                            {
                                recordDetail.ImagePath = $"{baseUrl}/{recordDetail.ImagePath.TrimStart('/')}";
                            }
                            if (!string.IsNullOrEmpty(recordDetail.EventDate))
                            {
                                recordDetail.EventDate = Convert.ToDateTime(recordDetail.EventDate).ToString("yyyy-MM-dd");
                            }
                                

                            events.Add(recordDetail);
                        }
                    }

                    int totalRecords = totalRecordsParam.Value != DBNull.Value ? Convert.ToInt32(totalRecordsParam.Value) : 0;
                    return new PagedResponse<EventResponse>
                    {
                        Data = events,
                        TotalRecords = totalRecords
                    };
                }
            }
        }

    }
}
