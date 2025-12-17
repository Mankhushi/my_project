using System.Text.Json.Serialization;

namespace MSINS_API.Models.Response
{
    public class BannerResponse
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int ID { get; init; }
        public string Type { get; init; }
        public string Name { get; init; }
        public string? Link { get; init; }
        public string? LinkType { get; init; }
        public string ImagePath { get; set; }
        public string isActive { get; init; }
    }
}
