namespace EndpointQueryService.Configurars
{
    public sealed class PrimaryConfigurar
    {
        private readonly IServiceCollection _services;
        private readonly IConfiguration _configuration;

        public PrimaryConfigurar(IServiceCollection services, IConfiguration configuration)
        {
            _services = services;
            _configuration = configuration;
        }

        public void Configure()
        {
            new AuthConfigurar().Configure(_services, _configuration);
            new CachingConfigurar().Configure(_services);
            new FirebaseConfigurar().Configure(_services, _configuration);
            new ServicesConfigurar().Configure(_services);
        }
    }
}
