using System.Text.Json.Serialization;

namespace MSINS_API.Models.Response
{
    public class InitiativeResponse
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int ID { get; init; }
        public string InitiativeName { get; init; }
        public string Description { get; init; }
        public string? Link { get; init; }
        public string? LinkType { get; init; }
        public string? ImagePath { get; set; }
        public string isActive { get; init; }
    }
}
