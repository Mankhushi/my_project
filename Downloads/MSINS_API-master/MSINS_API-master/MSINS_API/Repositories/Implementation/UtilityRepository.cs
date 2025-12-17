using AutoMapper;
using Microsoft.Data.SqlClient;
using MSINS_API.Models.Response;
using MSINS_API.POCO;
using MSINS_API.Repositories.Interface;
using SixLabors.ImageSharp;
using System.Data;

namespace MSINS_API.Repositories.Implementation
{
    public class UtilityRepository : IUtilityRepository
    {
        private readonly string _connectionString;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly BaseUrlSettings _baseUrlSettings;

        public UtilityRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");

        }

        //-----------------------------GET ALL TABLE---------------------------------------------
        public async Task<List<Dictionary<string, object>>> GetTableDataAsync(string tableName, string columns, string? orderColumn = null, string orderDirection = "ASC", bool? activeStatus = null)
        {
            var results = new List<Dictionary<string, object>>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("GetTableData", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Add parameters
                    command.Parameters.AddWithValue("@TableName", tableName);
                    command.Parameters.AddWithValue("@Columns", columns);
                    command.Parameters.AddWithValue("@OrderColumn", (object?)orderColumn ?? DBNull.Value);
                    command.Parameters.AddWithValue("@OrderDirection", orderDirection);
                    command.Parameters.AddWithValue("@ActiveStatus", (object?)activeStatus ?? DBNull.Value);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var row = new Dictionary<string, object>();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                row[reader.GetName(i)] = reader.IsDBNull(i) ? null : reader.GetValue(i);
                            }
                            results.Add(row);
                        }
                    }
                }
            }
            return results;
        }

    //-----------------------------GET ALL FILTEREDTABLE-------------------------------------
    public async Task<List<Dictionary<string, object>>> GetFilteredTableDataAsync(
    string tableName, string columns,
    string? orderColumn = null, string orderDirection = "ASC",
    string? conditionIntColumn = null, int? conditionIntValue = null,
    string? conditionStringColumn = null, string? conditionStringValue = null,
    string? conditionBoolColumn = null, int? conditionBoolValue = null,
    string? conditionBoolColumn1 = null, int? conditionBoolValue1 = null)
        {
            var results = new List<Dictionary<string, object>>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("GetFilteredTableData", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Add required parameters
                    command.Parameters.AddWithValue("@TableName", tableName);
                    command.Parameters.AddWithValue("@Columns", columns);
                    command.Parameters.AddWithValue("@OrderColumn", (object?)orderColumn ?? DBNull.Value);
                    command.Parameters.AddWithValue("@OrderDirection", orderDirection);

                    // Add dynamic condition parameters
                    command.Parameters.AddWithValue("@ConditionIntColumn", (object?)conditionIntColumn ?? DBNull.Value);
                    command.Parameters.AddWithValue("@ConditionIntValue", (object?)conditionIntValue ?? DBNull.Value);

                    command.Parameters.AddWithValue("@ConditionStringColumn", (object?)conditionStringColumn ?? DBNull.Value);
                    command.Parameters.AddWithValue("@ConditionStringValue", (object?)conditionStringValue ?? DBNull.Value);

                    command.Parameters.AddWithValue("@ConditionBoolColumn", (object?)conditionBoolColumn ?? DBNull.Value);
                    command.Parameters.AddWithValue("@ConditionBoolValue", (object?)conditionBoolValue ?? DBNull.Value);

                    command.Parameters.AddWithValue("@ConditionBoolColumn1", (object?)conditionBoolColumn1 ?? DBNull.Value);
                    command.Parameters.AddWithValue("@ConditionBoolValue1", (object?)conditionBoolValue1 ?? DBNull.Value);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var row = new Dictionary<string, object>();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                row[reader.GetName(i)] = reader.IsDBNull(i) ? null : reader.GetValue(i);
                            }
                            results.Add(row);
                        }
                    }
                }
            }
            return results;
        }


        // ------------------ GetAllSpeakersWithoutPaginationAsync -------------------
        public async Task<List<NewInitiativeSpeakerResponse>> GetAllSpeakersWithoutPaginationAsync(
             string? name, string? designation, bool? isActive)
        {
            var list = new List<NewInitiativeSpeakerResponse>();

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("usp_NewInitiativeSpeaker_GetAll_WithoutPagination", connection);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@Name", (object?)name ?? DBNull.Value);
            command.Parameters.AddWithValue("@Designation", (object?)designation ?? DBNull.Value);
            command.Parameters.AddWithValue("@IsActive", (object?)isActive ?? DBNull.Value);

            await connection.OpenAsync();

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new NewInitiativeSpeakerResponse
                {
                    SpeakerId = Convert.ToInt32(reader["SpeakerId"]),
                    Name = reader["Name"].ToString(),
                    Designation = reader["Designation"].ToString(),
                    ProfilePicUrl = reader["ProfilePicUrl"].ToString(),
                    IsActive = Convert.ToBoolean(reader["IsActive"])
                });
            }

            return list;
        }


        //-----------------------------GET ALL INITIATIVEMASTER-----------------------------------------------------
        public async Task<List<NewInitiativeMasterResponse>> GetInitiativeMasterAsync(
        string? title, bool? isActive)
        {
            var list = new List<NewInitiativeMasterResponse>();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            // FIXED: Correct stored procedure name
            using var command = new SqlCommand("GetAllInitiativeMaster_NoPagination", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@Title", (object?)title ?? DBNull.Value);
            command.Parameters.AddWithValue("@IsActive", isActive.HasValue ? (object)isActive.Value : DBNull.Value);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new NewInitiativeMasterResponse
                {
                    Id = reader.GetInt32(0),
                    Title = reader["Title"]?.ToString(),
                    HeaderImage = reader["HeaderImage"]?.ToString(),
                    Brief = reader["Brief"]?.ToString(),
                    Schedule = reader["Schedule"]?.ToString(),
                    Eligibility = reader["Eligibility"]?.ToString(),
                    Benefits = reader["Benefits"]?.ToString(),
                    Milestone = reader["Milestone"]?.ToString(),
                    Description = reader["Description"]?.ToString(),
                    IsActive = Convert.ToBoolean(reader["IsActive"])
                });
            }

            return list;
        }

        //--------------------------- GET ALL FAQS--------------------------------------------
        public async Task<List<NewFaqsResponse>> GetFaqsAsync(int? initiativeId, bool? isActive)
        {
            var list = new List<NewFaqsResponse>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("usp_Faqs_Listing", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@InitiativeId", (object?)initiativeId ?? DBNull.Value);
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


        // ------------------------------GET ALL  FundingPrograms     -----------------------------------
        //public async Task<List<NewFundingProgramsResponse>> GetFundingProgramsAsync(
        // string? fundingAgencyName, bool? isActive)
        //{
        //    var results = new List<NewFundingProgramsResponse>();

        //    using (var connection = new SqlConnection(_connectionString))
        //    {
        //        await connection.OpenAsync();

        //        using (var command = new SqlCommand("usp_FundingPrograms_Listing", connection))
        //        {
        //            command.CommandType = CommandType.StoredProcedure;

        //            command.Parameters.AddWithValue("@FundingAgencyName", (object?)fundingAgencyName ?? DBNull.Value);
        //            command.Parameters.AddWithValue("@IsActive", (object?)isActive ?? DBNull.Value);

        //            using (var reader = await command.ExecuteReaderAsync())
        //            {
        //                while (await reader.ReadAsync())
        //                {
        //                    results.Add(new NewFundingProgramsResponse
        //                    {
        //                        FundProgramId = Convert.ToInt32(reader["FundProgramId"]),
        //                        Logo = reader["Logo"]?.ToString(),
        //                        FundingAgencyName = reader["FundingAgencyName"]?.ToString(),
        //                        WebsiteLink = reader["WebsiteLink"]?.ToString(),
        //                        IsActive = Convert.ToBoolean(reader["IsActive"])
        //                    });
        //                }
        //            }
        //        }
        //    }

        //    return results;
        //}


        //-----------------GET ALL TYPEMASTER-------------------------------------
        public async Task<List<NewTypeMasterResponse>> GetAllTypesAsync(int sectorId, bool? isActive)
        {
            var list = new List<NewTypeMasterResponse>();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new SqlCommand("usp_TypeMaster_GetAll", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@SectorId", sectorId);
            command.Parameters.AddWithValue("@IsActive", (object?)isActive ?? DBNull.Value);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new NewTypeMasterResponse
                {
                    TypeId = Convert.ToInt32(reader["TypeId"]),
                    TypeName = reader["TypeName"].ToString(),
                    SectorName = reader["SectorName"].ToString(),
                    IsActive = Convert.ToBoolean(reader["IsActive"])
                });
            }

            return list;
        }
      public async Task<List<NewIncubatorsMasterResponse>> GetAllWithoutPaginationAsync(
 int? incubatorId,
 int? cityId,
 int? sectorId,
 int? typeId,
 bool? isActive)
  {
      var list = new List<NewIncubatorsMasterResponse>();

      using (var connection = new SqlConnection(_connectionString))
      {
          await connection.OpenAsync();

          using (var command = new SqlCommand("usp_NewIncubator_GetAll", connection))
          {
              command.CommandType = CommandType.StoredProcedure;

              command.Parameters.AddWithValue("@IncubatorId",
                  (object?)incubatorId ?? DBNull.Value);

              command.Parameters.AddWithValue("@CityId",
                  (object?)cityId ?? DBNull.Value);

              command.Parameters.AddWithValue("@SectorId",
                  (object?)sectorId ?? DBNull.Value);

              command.Parameters.AddWithValue("@TypeId",
                  (object?)typeId ?? DBNull.Value);

              // ⭐ Default IsActive = TRUE
              command.Parameters.AddWithValue("@IsActive",
                  (object?)(isActive ?? true));

              using (var reader = await command.ExecuteReaderAsync())
              {
                  while (await reader.ReadAsync())
                  {
                      list.Add(new NewIncubatorsMasterResponse
                      {
                          IncubatorId = Convert.ToInt32(reader["IncubatorId"]),
                          IncubatorName = reader["IncubatorName"]?.ToString(),
                          CityName = reader["CityName"]?.ToString(),
                          SectorName = reader["SectorName"]?.ToString(),
                          TypeName = reader["TypeName"]?.ToString(),
                          LogoPath = reader["LogoPath"]?.ToString(),
                          IsActive = Convert.ToBoolean(reader["IsActive"])
                      });
                  }
              }
          }
      }

      return list;
  }

        Task<List<NewIncubatorsMasterResponse>> IUtilityRepository.GetAllWithoutPaginationAsync(int? incubatorId, int? cityId, int? sectorId, int? typeId, bool? isActive)
        {
            return GetAllWithoutPaginationAsync(incubatorId, cityId, sectorId, typeId, isActive);
        }
    }
}
