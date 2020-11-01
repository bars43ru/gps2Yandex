using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Gps2Yandex.Core.Extensions;
using Gps2Yandex.Yandex.Configure;
using Gps2Yandex.Datasource.Configure;
using Gps2Yandex.Wialon.Configure;

namespace Gps2Yandex
{
    public class Programm
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host                
                .CreateDefaultBuilder(args)
                .ConfigureLogging(configureLogging =>
                {
                    configureLogging.AddConsole();
                    configureLogging.AddDebug();
                })
                .ConfigureHostConfiguration(config =>
                {
                    config
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("settings.json", optional: false, reloadOnChange: true)
                        .AddCommandLine(args);
                })
                .ConfigureServices((context, services) =>
                {
                    var config = context.Configuration;
                    services
                        .AddDataSetServices(config)
                        .AddModelServices(config)
                        .AddWialonServices(config)
                        .AddYadexServices(config);
                });
        }
    }
}
