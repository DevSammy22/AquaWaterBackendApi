using AquaWater.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AquaWater.Data.Extension
{
    public static class RegisterDbContext
    {
        public static void RegisterDBContext(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<AppDbContext>(options => options.UseNpgsql(config.GetConnectionString("DefaultConnection"),
            getAssembly => getAssembly.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)
            ));
        }
    }
}
