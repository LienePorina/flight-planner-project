namespace FlightPlanner.Exceptions
{
    public class FlightAlreadyExistsException : Exception
    {
        public FlightAlreadyExistsException() : base("Flight with the same details already exists.")
        {
        }
    }
}