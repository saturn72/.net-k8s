namespace Admin.Backend.Configurars
{
    public class ConfigurarRoot
    {
        internal void Configure(WebApplicationBuilder builder)
        {
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var services = builder.Services;
            var configuration = builder.Configuration;

            new AutoMapperConfigurar().Configure(services);
            new DataConfigurar().Configure(services, configuration);
            new ServicesConfigurar().Configure(services);
            new EasyCachingConfigurar().Configure(services);
        }
    }
}