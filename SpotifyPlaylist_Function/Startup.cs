using Microsoft.Extensions.Configuration;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using System.IO;

[assembly: FunctionsStartup(typeof(SpotifyPlaylist_Function.Startup))]
namespace SpotifyPlaylist_Function
{
    public class Startup : FunctionsStartup
    {
        public IConfigurationRoot Configuration { get; set; }

        public override void Configure(IFunctionsHostBuilder builder)
        {
            var basePath = Directory.GetCurrentDirectory();
            var config = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
            this.Configuration = config;
        }
    }
}
