using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Gps2Yandex.Model.Extensions;
using Gps2Yandex.Yandex.Configure;
using Gps2Yandex.Wialon.Configure;
using Serilog;

namespace Gps2Yandex
{
    public class Programm
    {
        const string LoggerFormat = "[{Level:u3}] {Timestamp:yyyy'/'MM'/'dd HH:mm:ss} | {Indent}{Message}{NewLine}{Exception}";
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
                    configureLogging.ClearProviders();

                    var serilogLogger = new LoggerConfiguration()
                        .WriteTo.File(
                            path: "logs/log-.txt",
                            outputTemplate: LoggerFormat,
                            fileSizeLimitBytes: null,
                            shared: true,
                            rollingInterval: RollingInterval.Hour)
                        .WriteTo.Console(outputTemplate: LoggerFormat)
                        .CreateLogger();

                    configureLogging.AddSerilog(serilogLogger);
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
                        .AddWialonServices(config)
                        .AddYadexServices(config);
                });
        }
    }
}
