namespace Datafeed.Rest.Configurars
{
    public sealed class ConfigurarRoot
    {
        private readonly IServiceCollection _services;

        public ConfigurarRoot(WebApplicationBuilder builder)
        {
            _services = builder.Services;
        }

        public void Configure()
        {
            new CachingConfigurar().Configure(_services);
        }
    }
}
