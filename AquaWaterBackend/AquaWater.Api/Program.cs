using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;

namespace AquaWater.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Mailjet.RunAsync().Wait();
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var isDevelopment = environment == Environments.Development;
            IConfiguration config = ConfigurationSetUp.GetConfig(isDevelopment);
            LogSettings.SetupSerilog(config);
            try
            {
                Log.Logger.Information("Applicatin starting up");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception e)
            {

                Log.Logger.Fatal(e, "The application failed to start correctly successfuly");
            }
            finally
            {
                Log.CloseAndFlush();
            }

            
        }
     
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
