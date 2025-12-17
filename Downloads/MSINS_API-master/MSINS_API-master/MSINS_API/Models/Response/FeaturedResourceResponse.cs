using System.Text.Json.Serialization;

namespace MSINS_API.Models.Response
{
    public class FeaturedResourceResponse
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int ID { get; init; }
        public string Title { get; init; } = string.Empty;
        public string FeaturedResourceDate { get; set; }
        public string Link { get; set; } = string.Empty;
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string IsActive { get; init; }
    }
}
