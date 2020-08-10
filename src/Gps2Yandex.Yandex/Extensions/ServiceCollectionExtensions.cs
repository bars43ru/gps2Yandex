using Microsoft.Extensions.DependencyInjection;

using Gps2Yandex.Yandex.Services;

namespace Gps2Yandex.Yandex.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApiYadex(this IServiceCollection serviceCollection)
            => serviceCollection.AddHostedService<Sending>();
    }
}
