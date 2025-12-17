using Microsoft.Data.SqlClient;
using MSINS_API.Models.Request;
using MSINS_API.Models.Response;
using MSINS_API.Repositories.Interface;
using System.Data;

namespace MSINS_API.Repositories.Implementation
{
    public class NewTestimonialsMasterRepository : INewTestimonialsMasterRepository
    {
        private readonly string _connectionString;

        public NewTestimonialsMasterRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // ============================================================
        //                     ADD TESTIMONIAL
        // ============================================================
        public async Task<(int Code, string Message)> AddTestimonialAsync(
            NewTestimonialsMasterRequest request,
            string? fileUrl)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var cmd = new SqlCommand("usp_Testimonials_Insert", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@TestimonyGivenBy", request.TestimonyGivenBy);
            cmd.Parameters.AddWithValue("@TextLineTwon", request.TextLineTwon ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Testimony", request.Testimony ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@IsActive", request.IsActive);
            cmd.Parameters.AddWithValue("@ProfilePic", fileUrl ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@InitiativeId", request.InitiativeId);
            cmd.Parameters.AddWithValue("@AdminId", request.AdminId);

            var resultCode = new SqlParameter("@ResultCode", SqlDbType.Int)
            { Direction = ParameterDirection.Output };

            var resultMsg = new SqlParameter("@ResultMessage", SqlDbType.NVarChar, 500)
            { Direction = ParameterDirection.Output };

            cmd.Parameters.Add(resultCode);
            cmd.Parameters.Add(resultMsg);

            await cmd.ExecuteNonQueryAsync();

            return (
                Convert.ToInt32(resultCode.Value),
                Convert.ToString(resultMsg.Value) ?? "Operation completed."
            );
        }

        // ============================================================
        //                     UPDATE TESTIMONIAL
        // ============================================================
        public async Task<(int Code, string Message)> UpdateTestimonialAsync(
            int testimonialId,
            NewTestimonialsMasterRequest request,
            string? fileUrl)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var cmd = new SqlCommand("usp_Testimonials_Update", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@TestimonialId", testimonialId);
            cmd.Parameters.AddWithValue("@TestimonyGivenBy", request.TestimonyGivenBy);
            cmd.Parameters.AddWithValue("@TextLineTwon", request.TextLineTwon ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Testimony", request.Testimony ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@IsActive", request.IsActive);
            cmd.Parameters.AddWithValue("@ProfilePic", fileUrl ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@InitiativeId", request.InitiativeId);
            cmd.Parameters.AddWithValue("@AdminId", request.AdminId);

            var resultCode = new SqlParameter("@ResultCode", SqlDbType.Int)
            { Direction = ParameterDirection.Output };

            var resultMsg = new SqlParameter("@ResultMessage", SqlDbType.NVarChar, 500)
            { Direction = ParameterDirection.Output };

            cmd.Parameters.Add(resultCode);
            cmd.Parameters.Add(resultMsg);

            await cmd.ExecuteNonQueryAsync();

            return (
                Convert.ToInt32(resultCode.Value),
                Convert.ToString(resultMsg.Value) ?? "Operation completed."
            );
        }

        // ============================================================
        //                      GET ALL TESTIMONIALS
        // ============================================================
        public async Task<List<NewTestimonialsMasterResponse>> GetTestimonialsAsync(
            int? initiativeId,
            bool? isActive)
        {
            var list = new List<NewTestimonialsMasterResponse>();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var cmd = new SqlCommand("usp_Testimonials_GetAll", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@InitiativeId", initiativeId ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@IsActive", isActive ?? (object)DBNull.Value);

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new NewTestimonialsMasterResponse
                {
                    TestimonialId = Convert.ToInt32(reader["TestimonialId"]),
                    TestimonyGivenBy = reader["TestimonyGivenBy"]?.ToString(),
                    TextLineTwon = reader["TextLineTwon"]?.ToString(),
                    Testimony = reader["Testimony"]?.ToString(),
                    IsActive = Convert.ToBoolean(reader["IsActive"]),
                    ProfilePic = reader["ProfilePic"]?.ToString(),   // RELATIVE PATH ONLY
                    InitiativeId = Convert.ToInt32(reader["InitiativeId"])
                });
            }

            return list;
        }

        // ============================================================
        //                    GET TESTIMONIAL BY ID
        // ============================================================
        public async Task<NewTestimonialsMasterResponse?> GetTestimonialByIdAsync(int testimonialId)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var cmd = new SqlCommand("usp_Testimonials_GetById", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@TestimonialId", testimonialId);

            using var reader = await cmd.ExecuteReaderAsync();
            if (!await reader.ReadAsync())
                return null;

            return new NewTestimonialsMasterResponse
            {
                TestimonialId = Convert.ToInt32(reader["TestimonialId"]),
                TestimonyGivenBy = reader["TestimonyGivenBy"]?.ToString(),
                TextLineTwon = reader["TextLineTwon"]?.ToString(),
                Testimony = reader["Testimony"]?.ToString(),
                IsActive = Convert.ToBoolean(reader["IsActive"]),
                ProfilePic = reader["ProfilePic"]?.ToString(),   // RELATIVE PATH
                InitiativeId = Convert.ToInt32(reader["InitiativeId"])
            };
        }
    }
}
