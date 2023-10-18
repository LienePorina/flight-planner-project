using FlightPlanner.Core.Models;
using FlightPlanner.Core.Services;
using FlightPlanner.Data;

namespace FlightPlanner.Services
{
    public class AirportService : EntityService<Airport>, IAirportService
    {
        public AirportService(IFlightPlannerDbContext context) : base(context)
        {
        }

        public Airport[] SearchAirports(string searchPhrase)
        {
            searchPhrase = searchPhrase.Trim().ToUpper();

            var matchingAirports = _context.Airports
                .AsEnumerable()
                .Where(a => a.Country.ToUpper().Contains(searchPhrase) ||
                            a.City.ToUpper().Contains(searchPhrase) ||
                            a.AirportCode.ToUpper().Contains(searchPhrase))
                .ToArray();

            return matchingAirports;
        }
    }
}
