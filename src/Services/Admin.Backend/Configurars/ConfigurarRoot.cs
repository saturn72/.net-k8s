using AnyService.Events.RabbitMQ;

namespace Admin.Backend.Configurars
{
    public class ConfigurarRoot
    {
        internal void Configure(WebApplicationBuilder builder)
        {
            builder.Services.AddControllers();

            var services = builder.Services;
            var configuration = builder.Configuration;

            new AutoMapperConfigurar().Configure(services);
            new DataConfigurar().Configure(services, configuration);
            new ServicesConfigurar().Configure(services);
            new EasyCachingConfigurar().Configure(services);
            new RabbitMqConfigurar().Configure(services, configuration);
        }
    }
}