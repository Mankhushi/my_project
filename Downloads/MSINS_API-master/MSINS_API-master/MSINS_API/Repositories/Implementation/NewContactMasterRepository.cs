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
    public class NewContactMasterRepository : INewContactMasterRepository
    {
        private readonly string _connectionString;
        private readonly IMapper _mapper;

        public NewContactMasterRepository(IConfiguration configuration, IMapper mapper)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _mapper = mapper;
        }

        // -------------------------------adde-------------------------------------------------


        // -------------------------------UPDATE CONTACT---------------------------------------------
        public async Task<(int Code, string Message)> UpdateContactAsync(NewContactMasteRequest model)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("usp_ContactMaster_Update", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@ContactId", model.ContactID);
                    command.Parameters.AddWithValue("@ContactNumber", model.ContactNumber);
                    command.Parameters.AddWithValue("@EmailAddress", model.EmailAddress);
                    command.Parameters.AddWithValue("@Address", model.Address);
                    command.Parameters.AddWithValue("@MapURL", (object?)model.MapURL ?? DBNull.Value);

                    var resultCode = new SqlParameter("@ResultCode", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };

                    var resultMessage = new SqlParameter("@ResultMessage", SqlDbType.NVarChar, 500)
                    {
                        Direction = ParameterDirection.Output
                    };

                    command.Parameters.Add(resultCode);
                    command.Parameters.Add(resultMessage);

                    await command.ExecuteNonQueryAsync();

                    return (
                        Code: Convert.ToInt32(resultCode.Value),
                        Message: resultMessage.Value?.ToString() ?? "Operation completed."
                    );
                }
            }
        }

        // ------------------------------GET CONTACT BY ID---------------------------------------------
        public async Task<NewContactMasterResponse?> GetContactByIdAsync(int contactId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("usp_ContactMaster_GetById", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ContactId", contactId);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            var response = new NewContactMasterResponse
                            {
                                ContactID = Convert.ToInt32(reader["ContactId"]),
                                ContactNumber = reader["ContactNumber"].ToString(),
                                EmailAddress = reader["EmailAddress"].ToString(),
                                Address = reader["Address"].ToString(),
                                MapURL = reader["MapURL"]?.ToString()
                            };

                            return response;
                        }
                    }
                }
            }

            return null; 
        }
    }
}
