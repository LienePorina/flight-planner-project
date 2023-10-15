using FlightPlanner.Core.Models;
using FlightPlanner.Core.Services;
using FlightPlanner.Data;
using Microsoft.EntityFrameworkCore;

namespace FlightPlanner.Services
{
    public class FlightService : EntityService<Flight>, IFlightService
    {
        public FlightService(IFlightPlannerDbContext context) : base(context)
        {
        }

        public Flight? GetFullFlightById(int id)
        {
            return _context.Flights.Include(f => f.To).Include(f => f.From).SingleOrDefault(f => f.Id == id);
        }

        public bool Exsists(Flight flight)
        {
            return _context.Flights.Any(f => f.ArrivalTime == flight.ArrivalTime && f.DepartureTime == flight.DepartureTime &&
                f.Carrier == flight.Carrier && f.To.AirportCode == flight.To.AirportCode && f.From.AirportCode == flight.From.AirportCode);
        }

        public void DeleteFlightById(int id)
        {
            Flight? flightToRemove = _context.Flights.FirstOrDefault(f => f.Id == id);

            if (flightToRemove != null)
            {
                _context.Flights.Remove(flightToRemove);

                if (flightToRemove.From != null)
                {
                    _context.Airports.Remove(flightToRemove.From);
                }

                if (flightToRemove.To != null)
                {
                    _context.Airports.Remove(flightToRemove.To);
                }
            }

            _context.SaveChanges();
        }

        public PageResult SearchFlights(SearchFlightsRequest request)
        {
            var matchingFlights = _context.Flights.Where(flight =>
                flight.From.AirportCode == request.From &&
                flight.To.AirportCode == request.To &&
                flight.DepartureTime.Contains(request.DepartureDate))
                .ToArray();

            int totalItems = matchingFlights.Length;

            var page = 0;
            if (totalItems > 0)
            {
                page = 1;
            }

            var pageResult = new PageResult
            {
                Page = page,
                TotalItems = totalItems,
                Items = matchingFlights
            };

            return pageResult;
        }
    }
}