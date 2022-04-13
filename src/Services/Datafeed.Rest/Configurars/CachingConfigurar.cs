using EasyCaching.Core;

namespace Datafeed.Rest.Configurars
{
    public class CachingConfigurar
    {
        private const string DefaultCachingProviderName = "default";
        public void Configure(IServiceCollection services)
        {
            services.AddEasyCaching(options =>
            {
                options.UseInMemory(DefaultCachingProviderName);
            });
            services.AddSingleton(sp =>
            sp.GetRequiredService<IEasyCachingProviderFactory>().GetCachingProvider(DefaultCachingProviderName));
        }
    }
}
