using System.ComponentModel.DataAnnotations;

namespace FlightPlanner.Models
{
    public class AirportRequest
    {
        [StringLength(100)]
        public string Country { get; set; }

        [StringLength(100)]
        public string City { get; set; }

        [StringLength(100)]
        public string Airport { get; set; }
    }
}
