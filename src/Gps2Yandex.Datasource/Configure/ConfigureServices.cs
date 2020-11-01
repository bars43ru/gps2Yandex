using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Gps2Yandex.Core.Interfaces;

using Gps2Yandex.Datasource.Handlers;
using Gps2Yandex.Datasource.Services;
using Gps2Yandex.Datasource.Entities;

namespace Gps2Yandex.Datasource.Configure
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddDataSetServices(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            return serviceCollection
                .Configure<Config>(options => configuration.GetSection("Catalogs").Bind(options))
                .AddTransient<RouteLoader>()
                .AddTransient<TransportLoader>()
                .AddTransient<ScheduleLoader>()
                .AddHostedService<MonitoringFiles>()
                .AddSingleton<Dataset>()
                .AddSingleton<IDataset>((sp) => sp.GetRequiredService<Dataset>());
        }
    }
}
