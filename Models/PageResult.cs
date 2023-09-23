using System.Text.Json.Serialization;

namespace FlightPlanner.Models
{
    public class PageResult
    {
        public int Page { get; set; }

        [JsonPropertyName("totalItems")]
        public int TotalItems { get; set; }

        public Flight[] Items { get; set; }
    }
}