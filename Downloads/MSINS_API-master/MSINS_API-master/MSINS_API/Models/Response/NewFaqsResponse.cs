using System.Text.Json.Serialization;

namespace MSINS_API.Models.Response
{
    public class NewFaqsResponse
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]

        public int FaqsId { get; set; }              // PK (Auto Increment)
        public int InitiativeId { get; set; }        // FK of InitiativeMaster
        public string Question { get; set; }         // NVARCHAR(255)
        public string Answer { get; set; }           // NVARCHAR(MAX)
        public bool IsActive { get; set; }           // Status Flag
    }
}
