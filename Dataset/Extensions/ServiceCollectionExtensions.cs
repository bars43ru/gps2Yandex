using Microsoft.Extensions.DependencyInjection;

using Gps2Yandex.Dataset.Services;

namespace Gps2Yandex.Dataset.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDataset(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                    .AddSingleton<Context>()
                    .AddTransient<RouteLoader>()
                    .AddTransient<TransportLoader>()
                    .AddTransient<ScheduleLoader>()
                    .AddHostedService<MonitoringFiles>();
        }
    }
}
