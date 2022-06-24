using Microsoft.Extensions.Configuration;
using System.IO;

namespace AquaWater.Api
{
    public class ConfigurationSetUp
    {
        public static IConfiguration GetConfig(bool isDevelopment)
        {
            return isDevelopment ? new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json")
                   .Build()

                :
                new ConfigurationBuilder()
                       .AddEnvironmentVariables()
                       .Build();
        }
    }
}
