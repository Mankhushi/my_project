using Microsoft.Data.SqlClient;
using MSINS_API.Models.Request;
using MSINS_API.Repositories.Interface;
using MSINS_API.Services.Implementation;
using System.Data;

namespace MSINS_API.Repositories.Implementation
{
    public class PublicConsultationRepository : IPublicConsultationRepository
    {
        private readonly string _connectionString;
        private readonly IFileUploadService _fileUploadService;
        public PublicConsultationRepository(IConfiguration configuration,IFileUploadService fileUploadService)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _fileUploadService = fileUploadService;
        }

        public async Task<int> SavePublicConsultationAsync(PublicationConsultationRequest model)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("SavePublicConsultation", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    string docURL = string.Empty;
                    if (model.file1 != null)
                    {
                        var uploadResponse = await _fileUploadService.UploadFileAsync(model.file1, "consultation");

                        if (!uploadResponse.IsSuccess)
                        {
                            return -5;
                        }

                        docURL = uploadResponse.FileUrl;
                    }

                    // Add input parameters
                    command.Parameters.AddWithValue("@FullName", model.FullName);
                    command.Parameters.AddWithValue("@Email", model.Email);
                    command.Parameters.AddWithValue("@ContactNumber", model.ContactNumber);
                    command.Parameters.AddWithValue("@CityDistrict", model.CityDistrict);
                    command.Parameters.AddWithValue("@OrganizationName", model.OrganizationName);
                    command.Parameters.AddWithValue("@ExpertiseSector", model.ExpertiseSector);
                    command.Parameters.AddWithValue("@ExpertiseSectorOther", model.ExpertiseSectorOther);
                    command.Parameters.AddWithValue("@AspectPolicy", model.AspectPolicy);
                    command.Parameters.AddWithValue("@Suggestion", model.Suggestion);
                    command.Parameters.AddWithValue("@Rating", model.Rating);
                    command.Parameters.AddWithValue("@RecommendateProgram", model.RecommendateProgram);
                    command.Parameters.AddWithValue("@file1", docURL);

                    // Add an output parameter for the status code
                    var statusParam = new SqlParameter("@Status", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(statusParam);

                    // Execute the stored procedure
                    await command.ExecuteNonQueryAsync();

                    // Retrieve the status code from the output parameter
                    int statusCode = Convert.ToInt32(statusParam.Value);

                    return statusCode;
                }
            }
        }
    }
}
