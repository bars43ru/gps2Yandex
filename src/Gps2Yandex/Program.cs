using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Gps2Yandex.Yandex.Extensions;
using Gps2Yandex.Model.Extensions;
using Gps2Yandex.Wialon.Extensions;

namespace Gps2Yandex
{
    class Programm
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureLogging(configureLogging =>
                {
                    configureLogging.AddConsole();
                    configureLogging.AddDebug();
                })
                .ConfigureHostConfiguration(config =>
                {
                    _ = config
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("settings.json", optional: false, reloadOnChange: true)
                            .AddCommandLine(args);
                })
                .ConfigureServices(services =>
                {
                    _ = services
                            .AddDataset()
                            .AddWialonListener()
                            .AddApiYadex();
                });
        }
    }
}
