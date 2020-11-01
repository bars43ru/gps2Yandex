using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Gps2Yandex.Yandex.Services;
using Gps2Yandex.Yandex.Entities;

namespace Gps2Yandex.Yandex.Configure
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddYadexServices(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            return serviceCollection
                .Configure<Config>(options => configuration.GetSection("Yandex").Bind(options))
                .AddHostedService<Sending>();
        }
    }
}
