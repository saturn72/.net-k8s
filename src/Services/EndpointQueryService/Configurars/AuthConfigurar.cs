namespace EndpointQueryService.Configurars
{
    public class AuthConfigurar
    {
        public void Configure(IServiceCollection services)
        {
            services.AddAuthentication();
        }
    }
}
