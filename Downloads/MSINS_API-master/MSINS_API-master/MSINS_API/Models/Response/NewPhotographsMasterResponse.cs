namespace MSINS_API.Models.Response
{
    public class NewPhotographsMasterResponse
    {
        public int PhotographsId { get; set; }

        public string? PhotographName { get; set; }

        // This will store the saved file URL/path
        public string PhotographPath { get; set; }

        public bool IsActive { get; set; }

        public int InitiativeId { get; set; }

       
    }
}
