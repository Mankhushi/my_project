using System.Text.Json.Serialization;

namespace MSINS_API.Models.Response
{
    public class PartnersResponse
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int ID { get; set; }
        public string Name { get; set; }
        public string? Link { get; set; }
        public string? LinkType { get; set; }
        public string ImagePath { get; set; }
        public string isActive { get; set; }
    }
}
