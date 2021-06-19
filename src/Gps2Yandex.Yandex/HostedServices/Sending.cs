using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Gps2Yandex.Model.Services;
using Gps2Yandex.Yandex.Handlers;
using Gps2Yandex.Yandex.Entities;
using System.Threading.Channels;

namespace Gps2Yandex.Yandex.HostedServices
{
    internal class Sending : BackgroundService
    {
        private ILogger Logger { get; }
        private Config Config { get; }
        private Context Context { get; }

        public Sending(
            ILogger<Sending> logger, 
            IOptions<Config> config, 
            Context context)
        {
            logger.LogInformation("Create service send data to Yandex.");
            Logger = logger;
            Config = config.Value;
            Context = context;
            ServicePointManager.Expect100Continue = false;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //Channel messages buffer
            Logger.LogInformation("Start service send data to Yandex.");
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(Config.Interval * 1000, stoppingToken);
                try
                {
                    Logger.LogInformation("Processing data for sending.");
                    var tracks = GetDataSet();
                    if (tracks.Items.Any())
                    {
                        await Send(tracks);
                    }
                    else
                    {
                        Logger.LogInformation("No data for sending to yandex.");
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "Error send to yandex.");
                }
            }

            stoppingToken.WaitHandle.WaitOne();
            Logger.LogInformation("Stopped service send data to Yandex.");
        }

        /// <summary>
        /// Получение данных с кэша
        /// </summary>
        /// <returns>DTO в виде модели yandex <see cref="Tracks"/> </returns>
        private Tracks GetDataSet()
        {
            var now = DateTime.Now;//.AddHours(-5);
            var schedules = Context.ActualSchedules(now, 30).ToList();
            // проверка а все ли маршруты есть в справочнике
            var lst = schedules.Where(a => !Context.Routes.Any(b => b.ExternalNumber == a.Route));
            if (lst.Any())
            {
                Logger.LogInformation("Find record in schedules where number rout not found in routes dataset.");
            }

            var routes = schedules
                             .Where(a => a.Begin <= now && now <= a.End)
                             .Join(Context.Routes,
                                   a => a.Route,
                                   b => b.ExternalNumber, (schedule, route) => new { Schedule = schedule, Route = route }).ToList();

            // проверка а все ли машины есть с гаражным номером которые пришли в распеисаниии
            var lst2 = schedules.Where(a => !Context.Buses.Any(b => b.ExternalNumber == a.Transport));
            if (lst2.Any())
            {
                Logger.LogInformation("Find record in schedules where number transport not found in buses dataset.");
            }
            var buses = routes
                            .Join(Context.Buses,
                                  a => a.Schedule.Transport,
                                  b => b.ExternalNumber,
                                  (data, bus) => new { Schedule = data.Schedule, Route = data.Route, Bus = bus }).ToList();


            var listTransport = Context
                                    .GpsPoints
                                    .Join(Context.Buses, a => a.MonitoringNumber, b => b.MonitoringNumber, (point, tc) => new { GpsPoint = point, TC = tc }).ToList();

            // проверка а все ли данные о записях uid wialon есть в справочнике что пришли к нам
            var lst3 = Context.GpsPoints.Where(a => !Context.Buses.Any(b => b.MonitoringNumber == a.MonitoringNumber));
            if (lst3.Any())
            {
                Logger.LogInformation("Find record in gps dataset where uid transport not found in buses dataset.");
            }

            var dataset = buses
                            .Join(Context.GpsPoints,
                                  a => a.Bus.MonitoringNumber,
                                  b => b.MonitoringNumber,
                                  (data, gps) => new { Schedule = data.Schedule, Route = data.Route, Bus = data.Bus, GpsData = gps })
                            .Where(a => a.GpsData.Time >= now.AddMinutes(-3))
                            .ToList();

            var result = dataset
                            .Select(a => new Track()
                            {
                                Uuid = a.Bus.MonitoringNumber,
                                Route = a.Route.YandexNumber,
                                VehicleType = (VehicleType)Enum.Parse(typeof(VehicleType), a.Bus.Type),
                                Point = new Point()
                                {
                                    Latitude = a.GpsData.Latitude,
                                    Longitude = a.GpsData.Longitude,
                                    AvgSpeed = a.GpsData.Speed,
                                    Direction = a.GpsData.Course,
                                    Time = a.GpsData.Time,
                                }
                            });
            var tracks = new Tracks(Config.Clid, result.ToArray());
            return tracks;
        }

        /// <summary>
        /// Отправка данных в yandex
        /// </summary>
        /// <param name="tracks"></param>
        private async Task Send(Tracks tracks)
        {
            var xml = XmlSerializer.Serialize(tracks);
            Dictionary<string, string> @params = new()
            {
                { "compressed", "0" },
                { "data", xml }
            };

            using var httpClient = new HttpClient();
            using var content = new FormUrlEncodedContent(@params!);
            content.Headers.Clear();
            content.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            HttpResponseMessage response = httpClient.PostAsync(Config.Host, content).Result;
            var result = await response.Content.ReadAsStringAsync();
            Logger.LogInformation($"Send data yandex. Response: {result}");
        }
    }
}
