using FlightPlanner.Models;
using Microsoft.AspNetCore.Server.IIS;
using System.Globalization;
using System.Net;

namespace FlightPlanner.Storage
{
    public class FlightStorage
    {
        private static List<Flight> _flightStorage = new List<Flight>();
        private static int _id = 0;

        public void AddFlight(Flight flight)
        {
            if (flight.From == null || flight.To == null || flight.Carrier == null || flight.Carrier == "" ||
                flight.DepartureTime == null || flight.ArrivalTime == null ||
                flight.From.Country == null || flight.From.City == null || flight.From.AirportCode == null ||
                flight.From.Country == "" || flight.From.City == "" || flight.From.AirportCode == "" ||
                flight.To.Country == null || flight.To.City == null || flight.To.AirportCode == null ||
                flight.To.Country == "" || flight.To.City == "" || flight.To.AirportCode == "")
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
    }
}
