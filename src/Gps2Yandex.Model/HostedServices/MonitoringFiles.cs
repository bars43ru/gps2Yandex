using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Gps2Yandex.References.Configure;
using Gps2Yandex.References.Handlers;
using Gps2Yandex.References.Services;

namespace Gps2Yandex.References.HostedServices
{
    /// <summary>
    /// Мониторит изменения в файлах "справочниках", и если произошли изменения в них вызывает их загрузчик
    /// </summary>
    /// MonitoringFileDataset
    internal class MonitoringFiles : BackgroundService
    {
        private ILogger Logger { get; }
        private Config Config { get; }
        private Context Context { get; }

        private FileInfo FileRoute => new(Path.Combine(BaseDirectory(), "route.txt"));
        private FileInfo FileTransport => new(Path.Combine(BaseDirectory(), "transport.txt"));
        private FileInfo FileSchedule => new(Path.Combine(BaseDirectory(), "schedule.txt"));

        public MonitoringFiles(
            ILogger<MonitoringFiles> logger, 
            IOptions<Config> config,
            Context context)
        {
            logger.LogInformation("Created a service for monitoring changes to files with the data set.");
            Logger = logger;
            Config = config.Value;
            Context = context;
        }

        private string BaseDirectory()
            => Path.GetFullPath(Environment.ExpandEnvironmentVariables(Config.Directory));

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Logger.LogInformation("Start service for monitoring changes to files with the data set.");
            return Task.Run(() =>
            {
                FirstReading();

                var fileProvider = new PhysicalFileProvider(BaseDirectory());                
                using var route = ChangeToken.OnChange(
                                       () => fileProvider.Watch(FileRoute.Name),
                                       () => RefreshingRoutes(FileRoute));

                using var transport = ChangeToken.OnChange(
                                       () => fileProvider.Watch(FileTransport.Name),
                                       () => RefreshingTransports(FileTransport));

                using var schedule = ChangeToken.OnChange(
                                       () => fileProvider.Watch(FileSchedule.Name),
                                       () => { RefreshingSchedules(FileSchedule); });

                stoppingToken.WaitHandle.WaitOne();
                Logger.LogInformation("Stopped service for monitoring changes to files with the data set.");
            }, stoppingToken);
        }

        private void FirstReading()
        {
            Logger.LogInformation($"First reading dataset from files.");
            try
            {
                Context.Update(RouteLoader.Read(FileRoute).ToArray());
                Context.Update(TransportLoader.Read(FileRoute).ToArray());
                Context.Update(ScheduleLoader.Read(FileRoute).ToArray());
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message, $"When trying first reading dataset from files.");
            }
        }

        private void RefreshingRoutes(FileInfo file)
        {
            Logger.LogInformation($"File `{file.Name}` was changed.");
            try
            {
                Context.Update(RouteLoader.Read(FileRoute).ToArray());
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message, $"When trying to update data based on `{file.Name}`.");
            }
        }

        private void RefreshingTransports(FileInfo file)
        {
            Logger.LogInformation($"File `{file.Name}` was changed.");
            try
            {
                Context.Update(TransportLoader.Read(FileRoute).ToArray());
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message, $"When trying to update data based on `{file.Name}`.");
            }
        }

        private void RefreshingSchedules(FileInfo file)
        {
            Logger.LogInformation($"File `{file.Name}` was changed.");
            try
            {
                Context.Update(ScheduleLoader.Read(FileRoute).ToArray());
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message, $"When trying to update data based on `{file.Name}`.");
            }
        }
    }
}