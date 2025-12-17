namespace MSINS_API.Models.Response
{
    public class NewFundingProgramsResponse
    {
        public int FundProgramId { get; set; }
        public string Logo { get; set; }          
        public string FundingAgencyName { get; set; }
        public string WebsiteLink { get; set; }
        public bool IsActive { get; set; }
    }
}
