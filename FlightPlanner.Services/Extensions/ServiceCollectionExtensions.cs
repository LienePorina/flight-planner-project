using FlightPlanner.Core.Models;
using FlightPlanner.Core.Services;
using FlightPlanner.Data;
using Microsoft.Extensions.DependencyInjection;

namespace FlightPlanner.Services.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddTransient<IFlightPlannerDbContext, FlightPlannerDbContext>();
            services.AddTransient<IDbService, DbService>();
            services.AddTransient<IEntityService<Airport>, EntityService<Airport>>();
            services.AddTransient<IEntityService<Flight>, EntityService<Flight>>();
            services.AddTransient<IFlightService, FlightService>();
            services.AddTransient<IAirportService, AirportService>();
            services.AddTransient<ICleanupService, CleanupService>();
        }
    }
}