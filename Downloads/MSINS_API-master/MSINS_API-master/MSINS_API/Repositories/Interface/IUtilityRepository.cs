using MSINS_API.Models.Response;

namespace MSINS_API.Repositories.Interface
{
    public interface IUtilityRepository
    {
        /// <summary>
        /// Retrieves table data dynamically based on the input parameters.
        /// </summary>
        /// <param name="tableName">The table from which to fetch data.</param>
        /// <param name="columns">Comma-separated string of columns to fetch.</param>
        /// <param name="orderColumn">Column to order by (optional).</param>
        /// <param name="orderDirection">Sort direction ('ASC' or 'DESC').</param>
        /// <param name="activeStatus">Filter by active status (optional).</param>
        /// <returns>A list of dictionary objects representing the table rows.</returns>
        Task<List<Dictionary<string, object>>> GetTableDataAsync(string tableName, string columns, string? orderColumn = null, string orderDirection = "ASC", bool? activeStatus = null);

        /// <summary>
        /// Fetches filtered table data based on dynamic conditions.
        /// </summary>
        /// <param name="tableName">The name of the table.</param>
        /// <param name="columns">Comma-separated columns to fetch.</param>
        /// <param name="orderColumn">Column to order by (optional).</param>
        /// <param name="orderDirection">Sort direction: "ASC" (default) or "DESC".</param>
        /// <param name="conditionIntColumn">Column name for integer filter.</param>
        /// <param name="conditionIntValue">Integer value for filtering.</param>
        /// <param name="conditionStringColumn">Column name for string filter.</param>
        /// <param name="conditionStringValue">String value for filtering.</param>
        /// <param name="conditionBoolColumn">Column name for boolean filter.</param>
        /// <param name="conditionBoolValue">Boolean value for filtering.</param>
        /// <returns>A list of dictionaries containing the filtered table data.</returns>
        Task<List<Dictionary<string, object>>> GetFilteredTableDataAsync(
            string tableName, string columns,
            string? orderColumn = null, string orderDirection = "ASC",
            string? conditionIntColumn = null, int? conditionIntValue = null,
            string? conditionStringColumn = null, string? conditionStringValue = null,
            string? conditionBoolColumn = null, int? conditionBoolValue = null,
             string? conditionBoolColumn1 = null, int? conditionBoolValue1 = null
        );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="designation"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        Task<List<NewInitiativeSpeakerResponse>> GetAllSpeakersWithoutPaginationAsync(string? name, string? designation, bool? isActive);
        Task<List<NewInitiativeMasterResponse>> GetInitiativeMasterAsync(string? title, bool? isActive);
        Task<List<NewFaqsResponse>> GetFaqsAsync(int? initiativeId, bool? isActive);


        //Task<List<NewFundingProgramsResponse>> GetFundingProgramsAsync(string? fundingAgencyName,bool? isActive);

        /// <summary>
        /// </summary>
        /// <param name="sectorId"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        Task<List<NewTypeMasterResponse>> GetAllTypesAsync(int sectorId, bool? isActive);

        Task<List<NewIncubatorsMasterResponse>> GetAllWithoutPaginationAsync(
int? incubatorId,
int? cityId,
int? sectorId,
int? typeId,
bool? isActive);

    }
}
