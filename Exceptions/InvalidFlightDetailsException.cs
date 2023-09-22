namespace FlightPlanner.Exceptions
{
    public class InvalidFlightDetailsException : Exception
    {
        public InvalidFlightDetailsException() : base("Invalid flight details")
        {
        }
    }
}
