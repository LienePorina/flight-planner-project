using FlightPlanner.Core.Models;

namespace FlightPlanner.Core.Services
{
    public interface IFlightService : IEntityService<Flight>
    {
        Flight? GetFullFlightById(int id);

        bool Exsists(Flight flight);

        public void DeleteFlightById(int id);

        PageResult SearchFlights(SearchFlightsRequest request);
    }
}
