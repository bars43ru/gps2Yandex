using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Gps2Yandex.Model.Services;
using Gps2Yandex.Model.Configure;

namespace Gps2Yandex.Model.Extensions
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddDataSetServices(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            return serviceCollection
                .Configure<Config>(options => configuration.GetSection("Catalogs").Bind(options))
                .AddSingleton<Context>()
                .AddTransient<RouteLoader>()
                .AddTransient<TransportLoader>()
                .AddTransient<ScheduleLoader>()
                .AddHostedService<MonitoringFiles>();
        }
    }
}
