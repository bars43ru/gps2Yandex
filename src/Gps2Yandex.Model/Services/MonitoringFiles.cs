using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Gps2Yandex.Model.Services
{
    /// <summary>
    /// Мониторит изменения в файлах "справочниках", и если произошли изменения в них вызывает их загрузчик
    /// </summary>
    /// MonitoringFileDataset
    internal class MonitoringFiles : BackgroundService
    {
        ILogger Logger { get; }
        IServiceProvider ServiceProvider { get; }
        Config Config { get; }

        private FileInfo FileRoute => new FileInfo(Path.Combine(BaseDirectory(), "route.txt"));
        private FileInfo FileTransport => new FileInfo(Path.Combine(BaseDirectory(), "transport.txt"));
        private FileInfo FileSchedule => new FileInfo(Path.Combine(BaseDirectory(), "schedule.txt"));

        public MonitoringFiles(ILogger<MonitoringFiles> logger, IServiceProvider serviceProvider, IConfiguration config)
        {
            logger.LogInformation("Created a service for monitoring changes to files with the data set.");
            Logger = logger;
            ServiceProvider = serviceProvider;
            Config = new Config();
            config.GetSection("Catalogs").Bind(Config);
        }

        private string BaseDirectory()
        {
            return Path.GetFullPath(Environment.ExpandEnvironmentVariables(Config.Directory));
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Logger.LogInformation("Start service for monitoring changes to files with the data set.");
            return Task.Run(() =>
            {
                FirstReading();

                var fileProvider = new PhysicalFileProvider(BaseDirectory());                
                using var route = ChangeToken.OnChange(
                                       () => fileProvider.Watch(FileRoute.Name),
                                       () => RouteLoader(FileRoute));

                using var transport = ChangeToken.OnChange(
                                       () => fileProvider.Watch(FileTransport.Name),
                                       () => TransportLoader(FileTransport));

                using var schedule = ChangeToken.OnChange(
                                       () => fileProvider.Watch(FileSchedule.Name),
                                       () => { ScheduleLoader(FileSchedule); });

                stoppingToken.WaitHandle.WaitOne();
                Logger.LogInformation("Stopped service for monitoring changes to files with the data set.");
            });
        }

        private void FirstReading()
        {
            Logger.LogInformation($"First reading dataset from files.");
            try
            {
                Create<RouteLoader>().LoadFrom(FileRoute);
                Create<TransportLoader>().LoadFrom(FileTransport);
                Create<ScheduleLoader>().LoadFrom(FileSchedule);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message, $"When trying first reading dataset from files.");
            }
        }

        private void RouteLoader(FileInfo file)
        {
            Logger.LogInformation($"File `{file.Name}` was changed.");
            try
            {
                Create<RouteLoader>().LoadFrom(file);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message, $"When trying to update data based on `{file.Name}`.");
            }
        }

        private void TransportLoader(FileInfo file)
        {
            Logger.LogInformation($"File `{file.Name}` was changed.");
            try
            {
                Create<TransportLoader>().LoadFrom(file);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message, $"When trying to update data based on `{file.Name}`.");
            }
        }

        private void ScheduleLoader(FileInfo file)
        {
            Logger.LogInformation($"File `{file.Name}` was changed.");
            try
            {
                Create<ScheduleLoader>().LoadFrom(file);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message, $"When trying to update data based on `{file.Name}`.");
            }
        }

        private T Create<T>()
        {            
            return ActivatorUtilities.GetServiceOrCreateInstance<T>(ServiceProvider);
        }
    }
}