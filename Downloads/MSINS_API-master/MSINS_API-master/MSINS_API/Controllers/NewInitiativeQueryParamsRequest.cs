namespace MSINS_API.Controllers
{
    public class NewInitiativeQueryParamsRequest
    {
        public object Title { get; internal set; }
        public bool IsActive { get; internal set; }
        public object PageIndex { get; internal set; }
        public object PageSize { get; internal set; }
    }
}