namespace MSINS_API.Models.Response
{
    public class NewWinnerMasterResponse
    {
        public int WinnerId { get; set; }

        public int SectorId { get; set; }   // Foreign Key to Sector Master

        public string? WinnerImage { get; set; }   // URL or path of uploaded file

        public string? WinnerName { get; set; }

        public bool IsActive { get; set; }

        public int InitiativeId { get; set; }
    }
}
