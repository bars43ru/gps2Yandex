using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Gps2Yandex.Model.Services;
using Gps2Yandex.Wialon.Extensions;

namespace Gps2Yandex.Wialon.Services
{
    /// <summary>
    /// Служба слушающая Wialon для принятия ретрансляционных пакетов
    /// </summary>
    internal class ListenerRetransmission : BackgroundService
    {
        ILogger Logger { get; }
        Config Config { get; }
        Context Context { get; }

        public ListenerRetransmission(ILogger<ListenerRetransmission> logger,  IConfiguration config, Context context)
        {
            logger.LogInformation("Created a service listener retransmission wialon.");

            Logger = logger;
            Config = new Config();
            Context = context;
            config.GetSection("WialonListen").Bind(Config);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Logger.LogInformation("Start a service listener retransmission wialon.");

            var tcpListener = new TcpListener(IPAddress.Parse(Config.Host), Config.Port);
            tcpListener.Start();
            stoppingToken.Register(() => tcpListener.Stop());
            while (!stoppingToken.IsCancellationRequested)
            {
                var tcpClient = await tcpListener.AcceptTcpClientAsync();
                _ = AcceptTcpClient(tcpClient);
            }

            Logger.LogInformation("Stopped a service listener retransmission wialon.");
        }

        private Task AcceptTcpClient(TcpClient tcpClient)
        {
            return Task.Run(() =>
            {
                var ipEndPoint = (IPEndPoint)tcpClient.Client.RemoteEndPoint;
                Logger.LogInformation($"Opening tcp connection. Remote end point `{ipEndPoint}`.");
                try
                {
                    using var source = tcpClient.GetStream();
                    foreach (var message in source.ReadAll())
                    {
                        Context.Update(message);
                    }
                }
                catch(Exception ex)
                {
                    Logger.LogError(ex, $"When receiving or processing data. Remote end point `{ipEndPoint}`.");
                }
                Logger.LogInformation($"Closed tcp connection. Remote end point `{ipEndPoint}`.");
            });
        }
    }
}
