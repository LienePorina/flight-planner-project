using System.Text.Json.Serialization;

namespace FlightPlanner.Models
{
    public class SearchFlightsRequest
    {
        [JsonPropertyName("from")]
        public string FromAirportCode { get; set; }

        [JsonPropertyName("to")]
        public string ToAirportCode { get; set; }

        public string DepartureDate { get; set; }
    }
}