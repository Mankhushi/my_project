using System.Text.Json.Serialization;

namespace MSINS_API.Models.Response
{
    public class LeadershipResponse
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int ID { get; init; }

        public string LeaderName { get; init; }

        public string Designation { get; init; }

        public string ImagePath { get; set; }

        public string isActive { get; init; }
    }
}
