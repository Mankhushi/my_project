namespace MSINS_API.Models.Response
{
    public class NewCityMasterResponse
    {
        internal int adminId;

        public int CityId { get; set; }
        public string CityName { get; set; }
        public bool IsActive { get; set; }
      
    }
}
