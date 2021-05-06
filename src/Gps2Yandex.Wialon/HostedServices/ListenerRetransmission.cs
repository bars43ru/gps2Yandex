using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Gps2Yandex.Model.Services;
using Gps2Yandex.Wialon.Entities;
using Gps2Yandex.Wialon.Handlers;

namespace Gps2Yandex.Wialon.HostedServices
{
    /// <summary>
    /// Служба слушающая Wialon для принятия ретрансляционных пакетов
    /// </summary>
    internal class ListenerRetransmission : BackgroundService
    {
        private ILogger Logger { get; }
        private Config Config { get; }
        private Context Context { get; }

        // счетчик активных tcp соединений установленных Wialon IPS
        private int activeConnections = 0;
        public int ActiveConnections { get => activeConnections; }

        public ListenerRetransmission(
            ILogger<ListenerRetransmission> logger,
            IOptions<Config> config,
            Context context)
        {
            logger.LogInformation("Created a service listener retransmission wialon.");

            Logger = logger;
            Config = config.Value;
            Context = context;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Logger.LogInformation("Start a service listener retransmission wialon.");
            try
            {

                var tcpListener = new TcpListener(IPAddress.Parse(Config.Host), Config.Port);
                tcpListener.Start();
                stoppingToken.Register(() =>
                {
                    tcpListener.Stop();
                });
                
                while (true)
                {
                    var tcpClient = await tcpListener.AcceptTcpClientAsync();
                    _ = ProcessingTcpClient(tcpClient, stoppingToken).ConfigureAwait(false);
                    stoppingToken.ThrowIfCancellationRequested();
                }
            }
            finally
            {
                Logger.LogInformation("Stopped a service listener retransmission wialon.");
            }
        }

        private async Task ProcessingTcpClient(TcpClient tcpClient, CancellationToken token)
        {
            var ipEndPoint = tcpClient.Client.RemoteEndPoint as IPEndPoint;
            Logger.LogInformation($"Opening tcp connection. Remote end point `{ipEndPoint}`.");
            Interlocked.Increment(ref activeConnections);
            try
            {
                try
                {
                    using var wialonIPS = new WialonIPSParse(tcpClient.GetStream());
                    await foreach(var message in wialonIPS.Messages(token).ConfigureAwait(false))
                    {
                        Context.Update(message);
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, $"When receiving or processing data. Remote end point `{ipEndPoint}`.");
                }
            }
            finally
            {
                tcpClient.Dispose();
                Logger.LogInformation($"Closed tcp connection. Remote end point `{ipEndPoint}`.");
                Interlocked.Decrement(ref activeConnections);
            }
        }
    }
}
