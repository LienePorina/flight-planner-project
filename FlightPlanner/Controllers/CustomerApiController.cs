using AutoMapper;
using FlightPlanner.Core.Models;
using FlightPlanner.Core.Services;
using FlightPlanner.Models;
using Microsoft.AspNetCore.Mvc;

namespace FlightPlanner.Controllers
{
    [Route("api")]
    [ApiController]
    public class CustomerApiController : ControllerBase
    {
        private readonly IAirportService _airportService;
        private readonly IFlightService _flightService;
        private readonly IMapper _mapper;

        public CustomerApiController(IAirportService airportService, IFlightService flightService, IMapper mapper)
        {
            _airportService = airportService;
            _flightService = flightService;
            _mapper = mapper;
        }

        [Route("airports")]
        [HttpGet]
        public IActionResult SearchAirports([FromQuery] string search)
        {
            Airport[] airports = _airportService.SearchAirports(search);

            AirportRequest[] airportRequests = airports.Select(airport => _mapper.Map<AirportRequest>(airport)).ToArray();

            return Ok(airportRequests);
        }

        [Route("flights/search")]
        [HttpPost]
        public IActionResult SearchFlights([FromBody] SearchFlightsRequest request)
        {
            if (request.From == request.To)
            {
                return BadRequest();
            }

            PageResult pageResult = _flightService.SearchFlights(request);

            return Ok(pageResult);
        }

        [Route("flights/{id}")]
        [HttpGet]
        public IActionResult FindFlightById(int id)
        {
            Flight? flight = _flightService.GetFullFlightById(id);

            FlightRequest flightRequest = _mapper.Map<FlightRequest>(flight);

            if (flightRequest == null)
            {
                return NotFound();
            }

            return Ok(flightRequest);
        }
    }
}