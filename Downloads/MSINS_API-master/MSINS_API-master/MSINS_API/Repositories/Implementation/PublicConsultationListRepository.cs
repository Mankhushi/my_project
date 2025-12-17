using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using MSINS_API.Models.Response;
using MSINS_API.POCO;
using MSINS_API.Repositories.Interface;
using System.Data;

namespace MSINS_API.Repositories.Implementation
{
    public class PublicConsultationListRepository: IPublicConsultationListRepository
    {
        private readonly string _connectionString;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly BaseUrlSettings _baseUrlSettings;

        public PublicConsultationListRepository(IHttpContextAccessor httpContextAccessor, IOptions<BaseUrlSettings> baseUrlSettings, IConfiguration configuration, IMapper mapper)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _baseUrlSettings = baseUrlSettings.Value;
        }

        public async Task<PagedResponse<PublicConsultationListResponse>> GetPublicConsultationAll(int pageIndex, int pageSize, string searchTerm, bool isExport)
        {
            var records = new List<PublicConsultationListResponse>();
            int totalRecords = 0;
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("GetAllPublicConsultation", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@PageIndex", pageIndex);
                    command.Parameters.AddWithValue("@PageSize", pageSize);
                    command.Parameters.AddWithValue("@SearchTerm", searchTerm);
                    command.Parameters.AddWithValue("@isExport", isExport);

                    // Add output parameter for the total count
                    var totalCountParam = new SqlParameter("@TotalCount", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(totalCountParam);

                    
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        
                        while (await reader.ReadAsync())
                        {
                            // Map the current reader row to the RecruiterListEFModel using AutoMapper
                            var recordDetail = _mapper.Map<PublicConsultationListResponse>(reader);
                            records.Add(recordDetail); // Add the mapped recruiter to the list

                            // Get Base URL dynamically from HttpContext
                            var request = _httpContextAccessor.HttpContext?.Request;
                            string baseUrl = request != null
                                ? $"{request.Scheme}://{request.Host}"
                                : _baseUrlSettings.Production;       // Production URL from settings


                            if (!string.IsNullOrEmpty(recordDetail.File1))
                            {
                                recordDetail.File1 = $"{baseUrl}/{recordDetail.File1.TrimStart('/')}";
                            }

                        }
                        
                    }
                    totalRecords = totalCountParam.Value != DBNull.Value ? Convert.ToInt32(totalCountParam.Value) : 0;
                }
            }

            return new PagedResponse<PublicConsultationListResponse>
            {
                Data = records,
                TotalRecords = totalRecords
            };
       
    }

    }
}
