using AquaWater.Data.Context;
using AquaWater.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace AquaWater.Data.Configuration
{
    public static class IdentityConfiguration
    {
        public static void ConfigurationIdentity(this IServiceCollection services)
        {
            services.AddIdentity<User, IdentityRole>(x =>
                {
                    x.Password.RequireUppercase = true;
                    x.Password.RequiredLength = 7;
                    x.Password.RequireDigit = false;
                    x.SignIn.RequireConfirmedEmail = true;
                })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();
        }
    }
}
