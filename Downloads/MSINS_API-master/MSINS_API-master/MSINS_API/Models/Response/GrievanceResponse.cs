namespace MSINS_API.Models.Response
{
    public class GrievanceResponse
    {
        public int ID { get; set; }
        public string FullName { get; set; }
        public string EmailAddress { get; set; }
        public string GrievanceType { get; set; }
        public string? Country { get; set; }
        public string? Subject { get; set; }
        public string? Description { get; set; }
        public string Mobile { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
