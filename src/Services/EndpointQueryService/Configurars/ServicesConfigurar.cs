using EndpointQueryService.Services.Endpoints;
using EndpointQueryService.Services.Events;
using EndpointQueryService.Services.Rate;
using EndpointQueryService.Services.Security.Permission;

namespace EndpointQueryService.Configurars
{
    public class ServicesConfigurar
    {
        public void Configure(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IEndpointService, EndpointService>();
            var cs = configuration.GetConnectionString("DefaultConnection");
            services.AddSingleton<IEndpointRepository>(sp => new DapperEndpointRepository(cs));

            services.AddSingleton<IPermissionManager, PermissionManager>();
            services.AddSingleton<IRateManager, RateManager>();
            services.AddSingleton<IRateRepository, DapperRateRepository>();

            services.AddSingleton<IEventBus, EventBus>();
        }
    }
}
