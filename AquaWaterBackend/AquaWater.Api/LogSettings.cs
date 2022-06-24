using Microsoft.Extensions.Configuration;
using Raven.Client.Documents;
using Serilog;
using Serilog.Events;
using System.Security.Cryptography.X509Certificates;
namespace AquaWater.Api
{
    public static class LogSettings
    {
        public static void SetupSerilog(IConfiguration config)
        {
            DocumentStore ravenStore = new DocumentStore
            {
                Urls = new string[] { config["RavenDBConfigurations:ConnectionURL"] },
                Database = config["RavenDBConfigurations:DatabaseName"],
            };

            ravenStore.Certificate = new X509Certificate2(config["RavenDBConfigurations:CertificateFilePath"],
            config["RavenDBConfigurations:Password"], X509KeyStorageFlags.MachineKeySet);

            ravenStore.Initialize();

            ILogger logger = new LoggerConfiguration()
                .WriteTo.RavenDB(ravenStore)
                .CreateLogger();

            Log.Logger = logger;
        }
    }
}