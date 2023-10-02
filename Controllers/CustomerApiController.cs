using FlightPlanner.Exceptions;
using FlightPlanner.Models;
using FlightPlanner.Storage;
using Microsoft.AspNetCore.Mvc;

namespace FlightPlanner.Controllers
{
    [Route("api")]
    [ApiController]
    public class CustomerApiController : ControllerBase
    {
        private readonly FlightStorage _storage;

        public CustomerApiController(FlightStorage storage)
        {
            _storage = storage;
        }

        [Route("airports")]
        [HttpGet]
        public IActionResult SearchAirports([FromQuery] string search)
        {
            var airports = _storage.SearchFlightByAirport(search);

            return Ok(airports);
        }

        [Route("flights/search")]
        [HttpPost]
        public IActionResult SearchFlights([FromBody] SearchFlightsRequest request)
        {
            try
            {
                var pageResult = _storage.SearchFlights(request);

                return Ok(pageResult);
            }
            catch (InvalidFlightDetailsException)
            {
                return StatusCode(400);
            }
        }

        [Route("flights/{id}")]
        [HttpGet]
        public IActionResult FindFlightById(int id)
        {
            var flight = _storage.GetFlight(id);

            if (flight == null)
            {
                return NotFound();
            }

            return Ok(flight);
        }
    }
}