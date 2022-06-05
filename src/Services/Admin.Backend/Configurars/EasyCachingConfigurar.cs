namespace Admin.Backend.Configurars
{
    public class EasyCachingConfigurar
    {
        private const string DefaultCachingProviderName = "default";
        public void Configure(IServiceCollection services)
        {
            services.AddEasyCaching(options =>
            {
                options.UseInMemory(DefaultCachingProviderName);
            });
        }
    }
}
