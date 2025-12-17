using Microsoft.AspNetCore.Http;

namespace MSINS_API.Models.Request
{
    public class NewWinnerMasterRequest
    {
        public int SectorId { get; set; }   // Foreign Key to Sector Master

        public IFormFile? WinnerImage { get; set; }   // nvarchar(max) → file upload

        public string? WinnerName { get; set; }       // nvarchar(255)

        public bool IsActive { get; set; }

        public int InitiativeId { get; set; }         // FK to Initiative Master

        public int AdminId { get; set; }
    }
}
