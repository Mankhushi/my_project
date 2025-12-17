using System.Text.Json.Serialization;

namespace MSINS_API.Models.Response
{
    public class TenderNotificationResponse
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int ID { get; init; }
        public string TenderId { get; init; }
        public string RefNo { get; init; } = string.Empty;
        public string Title { get; init; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string PublishedDate { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string IsActive { get; init; }
    }
}
