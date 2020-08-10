using Microsoft.Extensions.DependencyInjection;

using Gps2Yandex.Model.Services;

namespace Gps2Yandex.Model.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDataset(this IServiceCollection serviceCollection) 
            => serviceCollection
                    .AddSingleton<Context>()
                    .AddTransient<RouteLoader>()
                    .AddTransient<TransportLoader>()
                    .AddTransient<ScheduleLoader>()
                    .AddHostedService<MonitoringFiles>();
    }
}
