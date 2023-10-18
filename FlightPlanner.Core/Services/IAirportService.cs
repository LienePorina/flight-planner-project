using FlightPlanner.Core.Models;

namespace FlightPlanner.Core.Services
{
    public interface IAirportService : IEntityService<Airport>
    {
        Airport[] SearchAirports(string searchPhrase);
    }
}