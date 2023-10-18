using FlightPlanner.Core.Models;

namespace FlightPlanner.Core.Interfaces
{
    public interface IValidate
    {
        bool IsValid(Flight flight);
    }
}