using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using MSINS_API.Models.Request;
using MSINS_API.Models.Request;
using MSINS_API.Models.Response;
using MSINS_API.POCO;
using MSINS_API.Repositories.Interface;
using System.Data;

namespace MSINS_API.Repositories.Implementation
{
    public class NewFaqsRepository : INewFaqsRepository
    {
        private readonly string _connectionString;
        private readonly IMapper _mapper;

        public NewFaqsRepository(IConfiguration configuration, IMapper mapper)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _mapper = mapper;
        }

        // ================================================================
        //                          ADD FAQ
        // ================================================================
        public async Task<(int Code, string Message)> AddFaqAsync(NewFaqsRequest request)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("usp_Faqs_Insert", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@InitiativeId", request.InitiativeId);
                    command.Parameters.AddWithValue("@Question", request.Question);
                    command.Parameters.AddWithValue("@Answer", request.Answer);
                    command.Parameters.AddWithValue("@IsActive", request.IsActive);
                    command.Parameters.AddWithValue("@UserId", request.UserId);



                    var faqsIdParam = new SqlParameter("@FaqsId", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(faqsIdParam);

                    var resultCodeParam = new SqlParameter("@ResultCode", SqlDbType.Int)
                    { Direction = ParameterDirection.Output };
                    var resultMessageParam = new SqlParameter("@ResultMessage", SqlDbType.NVarChar, 500)
                    { Direction = ParameterDirection.Output };

                    command.Parameters.Add(resultCodeParam);
                    command.Parameters.Add(resultMessageParam);

                    await command.ExecuteNonQueryAsync();

                    return (
                        Convert.ToInt32(resultCodeParam.Value),
                        resultMessageParam.Value.ToString()
                    );
                }
            }
        }


        // ================================================================
        //                          UPDATE FAQ
        // ================================================================
        public async Task<(int Code, string Message)> UpdateFaqAsync(int faqsId, NewFaqsRequest request)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("usp_Faqs_Update", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@FaqsId", faqsId);
                    command.Parameters.AddWithValue("@InitiativeId", request.InitiativeId);
                    command.Parameters.AddWithValue("@Question", request.Question);
                    command.Parameters.AddWithValue("@Answer", request.Answer);
                    command.Parameters.AddWithValue("@IsActive", request.IsActive);
                    command.Parameters.AddWithValue("@UserId", request.UserId);

                    // ⭐ REQUIRED for logging
                    command.Parameters.AddWithValue("@UserId", request.UserId);

                    var resultCodeParam = new SqlParameter("@ResultCode", SqlDbType.Int)
                    { Direction = ParameterDirection.Output };

                    var resultMessageParam = new SqlParameter("@ResultMessage", SqlDbType.NVarChar, 500)
                    { Direction = ParameterDirection.Output };

                    command.Parameters.Add(resultCodeParam);
                    command.Parameters.Add(resultMessageParam);

                    await command.ExecuteNonQueryAsync();

                    int resultCode = Convert.ToInt32(resultCodeParam.Value);
                    string message = resultMessageParam.Value.ToString();

                    return (resultCode, message);
                }
            }
        }


        // ================================================================
        //                      GET ALL FAQS
        // ================================================================
        public async Task<List<NewFaqsResponse>> GetFaqsAsync(int? initiativeId, bool? isActive)
        {
            var list = new List<NewFaqsResponse>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("usp_Faqs_GetAll", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@InitiativeId", initiativeId);
                    command.Parameters.AddWithValue("@IsActive", (object?)isActive ?? DBNull.Value);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            list.Add(new NewFaqsResponse
                            {
                                FaqsId = Convert.ToInt32(reader["FaqsId"]),
                                InitiativeId = Convert.ToInt32(reader["InitiativeId"]),
                                Question = reader["Question"].ToString(),
                                Answer = reader["Answer"].ToString(),
                                IsActive = Convert.ToBoolean(reader["IsActive"])
                            });
                        }
                    }
                }
            }

            return list;
        }



        // ================================================================
        //                      GET FAQ BY ID
        // ================================================================
        public async Task<NewFaqsResponse?> GetFaqByIdAsync(int faqsId)
        {
            NewFaqsResponse? faq = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("usp_Faqs_GetById", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@FaqsId", faqsId);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            faq = new NewFaqsResponse
                            {
                                FaqsId = Convert.ToInt32(reader["FaqsId"]),
                                InitiativeId = Convert.ToInt32(reader["InitiativeId"]),
                                Question = reader["Question"].ToString(),
                                Answer = reader["Answer"].ToString(),
                                IsActive = Convert.ToBoolean(reader["IsActive"])
                            };
                        }
                    }
                }
            }

            return faq;
        }

    }
}
