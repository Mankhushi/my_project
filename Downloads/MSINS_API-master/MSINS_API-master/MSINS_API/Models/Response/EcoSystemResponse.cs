using System.Text.Json.Serialization;

namespace MSINS_API.Models.Response
{
    public class EcoSystemResponse
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int ID { get; set; }
        public string TextOne { get; set; }
        public string TextTwo { get; set; }
        public string ImagePath { get; set; }
        public string isActive { get; set; }
    }
}
