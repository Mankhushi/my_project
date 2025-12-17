namespace MSINS_API.Services.Interface
{
    public interface INewIncubatorsMasterService
    {
        Task<IncubatorsResultResponse> AddAsync(NewIncubatorsMasterRequest request);
        Task<IncubatorsResultResponse> UpdateAsync(NewIncubatorsMasterRequest request);
        Task<NewIncubatorsMasterResponse?> GetByIdAsync(int incubatorId);

        Task<List<NewIncubatorsMasterResponse>> GetAllWithoutPaginationAsync(
            string? citySearch,
            string? sectorSearch,
            string? typeSearch,
            bool? isActive);
    }
}
