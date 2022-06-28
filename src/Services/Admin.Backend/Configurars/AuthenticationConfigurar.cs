namespace Admin.Backend.Configurars
{
    public class AuthenticationConfigurar
    {
        public void Configure(IServiceCollection services)
        {
            services.AddAuthentication("Bearer");
        }
        }
}
