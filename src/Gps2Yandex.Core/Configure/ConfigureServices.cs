using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Gps2Yandex.Core.Services;

namespace Gps2Yandex.Core.Configure
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddModelServices(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            return serviceCollection
                .AddSingleton<Context>();
        }
    }
}
