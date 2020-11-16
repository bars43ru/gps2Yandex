using Microsoft.Extensions.DependencyInjection;

namespace Gps2Yandex.WebApi.Extensions
{
    public static class ConfigureServices
    {
        public static void AddWebApiServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddControllers();
        }
    }
}
