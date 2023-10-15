using FlightPlanner.Core.Interfaces;
using FlightPlanner.Validations;

namespace FlightPlanner.Extensions
{
    public static class ValidatorCollectionExtensions
    {
        public static void RegisterValidators(this IServiceCollection services)
        {
            services.AddTransient<IValidate, FlightValuesValidator>();
            services.AddTransient<IValidate, AirportValuesValidator>();
            services.AddTransient<IValidate, SameAirportValidator>();
            services.AddTransient<IValidate, FlightDatesValidator>();
        }
    }
}
