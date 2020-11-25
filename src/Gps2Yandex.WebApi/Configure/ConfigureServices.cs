using Microsoft.Extensions.DependencyInjection;

namespace Gps2Yandex.WebApi.Configure
{
    public static class ConfigureServices
    {
        public static void AddWebApiServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddControllers();
        }
    }
}
