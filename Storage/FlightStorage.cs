using FlightPlanner.Models;
using System.Globalization;

namespace FlightPlanner.Storage
{
    public class FlightStorage
    {
        private static List<Flight> _flightStorage = new List<Flight>();
        private static int _id = 0;

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
                flight.Id = _id++;
                _flightStorage.Add(flight);
            }
        }

        public void DeleteFlight(int flightId)
        {
            Flight? flightToRemove = _flightStorage.FirstOrDefault(f => f.Id == flightId);

            if (flightToRemove != null)
            {
                _flightStorage.Remove(flightToRemove);
            }
        }

        public bool FlightExists(Flight flight)
        {
            return _flightStorage.Any(existingFlight =>
                existingFlight.From.Country == flight.From.Country &&
                existingFlight.From.City == flight.From.City &&
                existingFlight.From.AirportCode == flight.From.AirportCode &&
                existingFlight.ArrivalTime == flight.ArrivalTime);
        }

        public void Clear()
        {
            _flightStorage.Clear();
        }

        public Airport[] SearchFlightByAirport(string searchPhrase)
        {
            searchPhrase = searchPhrase.Trim().ToUpper();

            List<Airport> matchingAirports = _flightStorage.SelectMany(f => new List<Airport> { f.From, f.To })
                .Where(airport =>
                airport.Country.ToUpper().Contains(searchPhrase) ||
                airport.City.ToUpper().Contains(searchPhrase) ||
                airport.AirportCode.ToUpper().Contains(searchPhrase))
                .ToList();

            return matchingAirports.ToArray();
        }

        public PageResult SearchFlights(SearchFlightsRequest request)
        {
            if (request.FromAirportCode == request.ToAirportCode)
            {
                throw new Exceptions.InvalidFlightDetailsException();
            }

            var matchingFlights = _flightStorage.Where(flight =>
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

        public Flight FindFlightById(int id)
        {
            var flight = _flightStorage.FirstOrDefault(flight => flight.Id == id);

            return flight;
        }
    }
}