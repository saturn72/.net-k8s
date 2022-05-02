namespace EndpointQueryService.Configurars
{
    public sealed class PrimaryConfigurar
    {
        private readonly IServiceCollection _services;
        private readonly IConfiguration _configuration;

        public PrimaryConfigurar(WebApplicationBuilder builder)
        {
            _services = builder.Services;
            _configuration = builder.Configuration;
        }

        public void Configure()
        {
            new AuthConfigurar().Configure(_services);
            new CachingConfigurar().Configure(_services);
            new ServicesConfigurar().Configure(_services, _configuration);
        }
    }
}
