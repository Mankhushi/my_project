using System.Text.Json.Serialization;

namespace MSINS_API.Models.Response
{
    public class EventResponse
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int EventId { get; init; }

        public string EventType { get; init; }
        public string EventName { get; init; }

        public string EventDate { get; set; }

        public string EventLocation { get; init; }

        public string ImagePath { get; set; }

        public string EventDesc { get; init; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string IsActive { get; init; }
    }
}
