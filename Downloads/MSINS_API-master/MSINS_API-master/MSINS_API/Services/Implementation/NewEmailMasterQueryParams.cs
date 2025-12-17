namespace MSINS_API.Services.Implementation
{
    public class NewEmailMasterQueryParams
    {
        public int PageNumber { get; internal set; }
        public int PageSize { get; internal set; }
        public string? SearchTerm { get; internal set; }
        public bool? IsActive { get; internal set; }
    }
}