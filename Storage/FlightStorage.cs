using FlightPlanner.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace FlightPlanner.Storage
{
    public class FlightStorage
    {
        private readonly FlightPlannerDbContext _context;

        public FlightStorage(FlightPlannerDbContext context)
        {
            _context = context;
        }

        public void AddFlight(Flight flight)
        {
            if (flight.From == null || flight.To == null || string.IsNullOrEmpty(flight.Carrier) ||
                string.IsNullOrEmpty(flight.DepartureTime) || string.IsNullOrEmpty(flight.ArrivalTime) ||
                string.IsNullOrEmpty(flight.From.Country) || string.IsNullOrEmpty(flight.From.City) || string.IsNullOrEmpty(flight.From.AirportCode) ||
                string.IsNullOrEmpty(flight.To.Country) || string.IsNullOrEmpty(flight.To.City) || string.IsNullOrEmpty(flight.To.AirportCode))
            {
                throw new Exceptions.InvalidFlightDetailsException();
            }

            if (flight.To.Country.Trim().ToLower() == flight.From.Country.Trim().ToLower() &&
                flight.To.City.Trim().ToLower() == flight.From.City.Trim().ToLower() &&
                flight.To.AirportCode.Trim().ToLower() == flight.From.AirportCode.Trim().ToLower())
            {
                throw new Exceptions.InvalidFlightDetailsException();
            }

            DateTime.TryParseExact(flight.ArrivalTime, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime arrivalTime);
            DateTime.TryParseExact(flight.DepartureTime, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime departureTime);

            if (flight.ArrivalTime == flight.DepartureTime || arrivalTime < departureTime)
            {
                throw new Exceptions.InvalidFlightDetailsException();
            }

            if (FlightExists(flight))
            {
                throw new Exceptions.FlightAlreadyExistsException();
            }
            else
            {
                _context.Flights.Add(flight);
                _context.SaveChanges();
            }
        }

        public void DeleteFlight(int flightId)
        {
            Flight? flightToRemove = _context.Flights.FirstOrDefault(f => f.Id == flightId);

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

        public bool FlightExists(Flight flight)
        {
            return _context.Flights.Any(existingFlight =>
                existingFlight.From.Country == flight.From.Country &&
                existingFlight.From.City == flight.From.City &&
                existingFlight.From.AirportCode == flight.From.AirportCode &&
                existingFlight.ArrivalTime == flight.ArrivalTime);
        }

        public void Clear()
        {
            _context.Flights.RemoveRange(_context.Flights);
            _context.Airports.RemoveRange(_context.Airports);
            _context.SaveChanges();
        }

        public Airport[] SearchFlightByAirport(string searchPhrase)
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

        public PageResult SearchFlights(SearchFlightsRequest request)
        {
            if (request.FromAirportCode == request.ToAirportCode)
            {
                throw new Exceptions.InvalidFlightDetailsException();
            }

            var matchingFlights = _context.Flights.Where(flight =>
                flight.From.AirportCode == request.FromAirportCode &&
                flight.To.AirportCode == request.ToAirportCode &&
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

        public Flight GetFlight(int id)
        {
            return _context.Flights.Include(f => f.From).Include(f => f.To).SingleOrDefault(f => f.Id == id);
        }
    }
}