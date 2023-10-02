using FlightPlanner.Exceptions;
using FlightPlanner.Models;
using FlightPlanner.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlightPlanner.Controllers
{
    [Authorize]
    [Route("admin-api")]
    [ApiController]
    public class AdminApiController : ControllerBase
    {
        private readonly FlightStorage _storage;
        private readonly static object _locker = new();

        public AdminApiController(FlightStorage storage)
        {
            _storage = storage;
        }

        [Route("flights/{id}")]
        [HttpGet]
        public IActionResult GetFlight(int id)
        {
            var flight = _storage.GetFlight(id);

            if (flight == null)
            {
                return NotFound();
            }

            return Ok(flight);
        }

        [Route("flights")]
        [HttpPut]
        public IActionResult PutFlight(Flight flight)
        {
            try
            {
                lock (_locker)
                {
                    _storage.AddFlight(flight);
                    return Created("", flight);
                }
            }
            catch (FlightAlreadyExistsException)
            {
                return StatusCode(409);
            }
            catch (InvalidFlightDetailsException)
            {
                return StatusCode(400);
            }
        }

        [Route("flights/{id}")]
        [HttpDelete]
        public IActionResult DeleteFlight(int id)
        {
            lock (_locker)
            {
                _storage.DeleteFlight(id);

                return Ok();
            }
        }
    }
}