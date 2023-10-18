using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FlightPlanner.Data
{
    public static class DbServiceExtensions
    {
        public static void RegisterDbService(this IServiceCollection service, IConfiguration configuration)
        {
            service.AddDbContext<FlightPlannerDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("flight-planner")));
        }
    }
}
