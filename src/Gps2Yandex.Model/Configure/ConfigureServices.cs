using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Gps2Yandex.References.Services;
using Gps2Yandex.References.Configure;
using Gps2Yandex.References.HostedServices;

namespace Gps2Yandex.References.Extensions
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddDataSetServices(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            return serviceCollection
                .Configure<Config>(options => configuration.GetSection("Catalogs").Bind(options))
                .AddSingleton<Context>()
                .AddHostedService<MonitoringFiles>();
        }
    }
}
