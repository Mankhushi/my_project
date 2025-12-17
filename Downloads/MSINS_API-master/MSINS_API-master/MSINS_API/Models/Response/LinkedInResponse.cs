using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MSINS_API.Models.Response
{
    public class LinkedInResponse
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int ID { get; init; }
        
        public string Link { get; init; }
        
        public string Title { get; init; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string IsActive { get; init; }
    }
}
