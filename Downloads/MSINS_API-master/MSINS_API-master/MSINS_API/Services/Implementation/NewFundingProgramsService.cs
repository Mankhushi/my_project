using MSINS_API.Models.Request;
using MSINS_API.Models.Response;
using MSINS_API.Repositories.Interface;
using MSINS_API.Services.Interface;
using System.Net;

namespace MSINS_API.Services.Implementation
{
    public class NewFundingProgramsService : INewFundingProgramsService
    {
        private readonly INewFundingProgramsRepository _fundingRepository;

        public NewFundingProgramsService(INewFundingProgramsRepository fundingRepository)
        {
            _fundingRepository = fundingRepository;
        }

        // ============================================================
        //                    ADD FUNDING PROGRAM
        // ============================================================
        public async Task<(int Code, string Message)> AddFundingProgramAsync(NewFundingProgramsRequest request)
        {
            // Validation
            if (string.IsNullOrWhiteSpace(request.FundingAgencyName))
                return ((int)HttpStatusCode.BadRequest, "Funding Agency Name is required.");

            // DB Call
            var (resultCode, message) = await _fundingRepository.AddFundingProgramAsync(request);

            if (resultCode == 1)
                return (200, message);

            return (500, "Failed to add funding program.");
        }

        // ============================================================
        //                    UPDATE FUNDING PROGRAM
        // ============================================================
        public async Task<(int Code, string Message)> UpdateFundingProgramAsync(int fundProgramId, NewFundingProgramsRequest request)
        {
            if (fundProgramId <= 0)
                return ((int)HttpStatusCode.BadRequest, "Invalid FundProgramId.");

            if (string.IsNullOrWhiteSpace(request.FundingAgencyName))
                return ((int)HttpStatusCode.BadRequest, "Funding Agency Name is required.");

            var (resultCode, message) = await _fundingRepository.UpdateFundingProgramAsync(fundProgramId, request);

            if (resultCode == 1)
                return (200, message);

            if (resultCode == 0)
                return (404, "Funding program not found.");

            return (500, "Failed to update funding program.");
        }

        // ============================================================
        //              GET ALL FUNDING PROGRAMS (PAGINATION)
        // ============================================================
      public async Task<PagedResponse<NewFundingProgramsResponse>> GetFundingProgramsAsync(
    string? fundingAgencyName,
    bool? isActive,
    int pageIndex,
    int pageSize)
        {
            return await _fundingRepository.GetFundingProgramsAsync(
                fundingAgencyName,
                isActive,
                pageIndex,
                pageSize
            );
        }


        // ============================================================
        //                   GET FUNDING PROGRAM BY ID
        // ============================================================
        public async Task<NewFundingProgramsResponse?> GetFundingProgramByIdAsync(int fundProgramId)
        {
            if (fundProgramId <= 0)
                return null;

            return await _fundingRepository.GetFundingProgramByIdAsync(fundProgramId);
        }
    }
}
