using System.ComponentModel.DataAnnotations;

namespace FlightPlanner.Core.Models
{
    public class Airport : Entity
    {
        [StringLength(100)]
        public string Country { get; set; }

        [StringLength(100)]
        public string City { get; set; }

        [StringLength(100)]
        public string AirportCode { get; set; }
    }
}