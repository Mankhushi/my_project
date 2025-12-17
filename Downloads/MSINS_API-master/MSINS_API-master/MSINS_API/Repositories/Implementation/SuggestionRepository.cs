using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MSINS_API.Models.Request;
using MSINS_API.Models.Response;
using MSINS_API.Repositories.Interface;
using System.Data;
using System.Net.Mail;

namespace MSINS_API.Repositories.Implementation
{
    public class SuggestionRepository : ISuggestionRepository
    {
        private readonly string _connectionString;
        private readonly IMapper _mapper;

        public SuggestionRepository(IConfiguration configuration, IMapper mapper)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _mapper = mapper;
        }
        public async Task<int> ExecuteSuggestionProcedureAsync(SuggestionRequestModel request)
        {
            string errorMessage = string.Empty;
            int statusCode = 500;           
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("AddSuggesstion", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@fullName", request.fullName);
                    command.Parameters.AddWithValue("@email", request.email);
                    command.Parameters.AddWithValue("@suggestionType", request.suggestionType);
                    command.Parameters.AddWithValue("@countryCode", request.countryCode);
                    command.Parameters.AddWithValue("@mobile", request.mobile);
                    command.Parameters.AddWithValue("@subject", request.subject);
                    command.Parameters.AddWithValue("@description", request.description);

                    // Add an output parameter for the result message
                    var statusParam = new SqlParameter("@Status", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(statusParam);

                    await command.ExecuteNonQueryAsync();

                    // Get the result message from the output parameter                    
                    statusCode = Convert.ToInt32(statusParam.Value);


                    return statusCode;
                }
            }
        }

        public async Task<PagedResponse<SuggestionResponse>> GetAllSuggestion(int pageIndex, int pageSize, string searchTerm, bool isExport)
        {
            var records = new List<SuggestionResponse>();
            int totalRecords = 0;
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("GetAllSuggestion", connection))
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
                            var recordDetail = _mapper.Map<SuggestionResponse>(reader);
                            records.Add(recordDetail); // Add the mapped recruiter to the list


                        }

                    }
                    totalRecords = totalCountParam.Value != DBNull.Value ? Convert.ToInt32(totalCountParam.Value) : 0;
                }
            }

            return new PagedResponse<SuggestionResponse>
            {
                Data = records,
                TotalRecords = totalRecords
            };
        }
    }
}
