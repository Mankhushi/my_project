using System.Text.Json.Serialization;

namespace MSINS_API.Models.Response
{
    public class MediaResponse
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int ID { get; init; }

        public string Link { get; init; }
        public string MediaName { get; init; }

        public string MediaDate { get; init; }

        public string ImagePath { get; set; }

        public string MediaDesc { get; init; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string IsActive { get; init; }
    }
}
