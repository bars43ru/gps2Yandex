using System.IO;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;

using Gps2Yandex.Core.Configure;
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
            Directory.SetCurrentDirectory(path: Path.GetDirectoryName(typeof(Programm).Assembly.Location)!);

            return Host                
                .CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureLogging(configureLogging =>
                {
                    configureLogging.AddConsole();
                    configureLogging.AddDebug();
                })
                .ConfigureHostConfiguration(config =>
                {
                    config
                        .SetBasePath(Directory.GetCurrentDirectory())// Path.GetDirectoryName(typeof(Programm).Assembly.Location))
                        .AddJsonFile("settings.json", optional: false, reloadOnChange: true)
                        .AddEnvironmentVariables()
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
