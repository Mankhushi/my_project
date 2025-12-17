using System.Text.Json.Serialization;

namespace MSINS_API.Models.Response
{
    public class GeneralBodyResponse
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int ID { get; init; }
        public string Name { get; init; }
        public string Position { get; init; }
        public string Designation { get; init; }
        public string Address { get; init; }
        public string IsActive { get; init; }
    }
}
