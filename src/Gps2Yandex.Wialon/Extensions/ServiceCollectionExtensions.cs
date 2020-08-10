using Microsoft.Extensions.DependencyInjection;

using Gps2Yandex.Wialon.Services;

namespace Gps2Yandex.Wialon.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWialonListener(this IServiceCollection serviceCollection) 
            => serviceCollection.AddHostedService<ListenerRetransmission>();
    }
}
