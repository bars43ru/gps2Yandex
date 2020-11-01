using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Gps2Yandex.Wialon.Entities;
using Gps2Yandex.Wialon.Services;

namespace Gps2Yandex.Wialon.Configure
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddWialonServices(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            return serviceCollection
                .Configure<Config>(options => configuration.GetSection("WialonListen").Bind(options))
                .AddHostedService<ListenerRetransmission>();
        }
    }
}
