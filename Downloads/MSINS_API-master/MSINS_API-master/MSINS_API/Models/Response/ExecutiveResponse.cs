using System.Text.Json.Serialization;

namespace MSINS_API.Models.Response
{
    public class ExecutiveResponse
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int ID { get; init; }
        public string ImagePath { get; set; }
        public string IsActive { get; init; }
        public string Description { get; init; }
        public string Name { get; init; }
    }
}
