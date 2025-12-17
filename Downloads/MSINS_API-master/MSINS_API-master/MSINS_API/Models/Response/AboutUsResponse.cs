using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MSINS_API.Models.Response
{
    public class AboutUsResponse
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int ID { get; init; }
        
        public string TextOne { get; init; }
        
        public string TextTwo { get; init; }
        
        public string ImageFile { get; set; }
        
        public string PDFFile { get; set; }
    }
}
