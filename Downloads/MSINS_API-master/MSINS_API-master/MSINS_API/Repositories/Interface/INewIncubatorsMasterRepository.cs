using MSINS_API.Models.Response;

public interface INewIncubatorsMasterRepository
{
    Task<(bool IsSuccess, string Message, int IncubatorId)> AddAsync(NewIncubatorsMasterRequest request, string? logoPath);
    Task<(bool IsSuccess, string Message, int IncubatorId)> UpdateAsync(NewIncubatorsMasterRequest request, string? logoPath);
    Task<NewIncubatorsMasterResponse?> GetByIdAsync(int incubatorId);

    Task<List<NewIncubatorsMasterResponse>> GetAllWithoutPaginationAsync(
    string? citySearch,
    string? sectorSearch,
    string? typeSearch,
    bool? isActive);

}
