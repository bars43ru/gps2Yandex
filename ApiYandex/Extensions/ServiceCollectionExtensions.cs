using Microsoft.Extensions.DependencyInjection;

using Gps2Yandex.ApiYandex.Services;

namespace Gps2Yandex.ApiYandex.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApiYadex(this IServiceCollection serviceCollection)
        {
            return serviceCollection.AddHostedService<Sending>();
        }
    }
}
