using Admin.Backend.Domain;
using Admin.Backend.Services.Endpoint;
using Admin.Backend.Services.Events;
using Admin.Backend.Services.Security;

namespace Admin.Backend.Configurars
{
    public class ServicesConfigurar
    {
        public void Configure(IServiceCollection services)
        {
            services.AddSingleton<IEndpointService, EndpointService>();
            services.AddSingleton<IValidator<CreateContext<EndpointDomainModel>>, EndpointValidator>();

            services.AddSingleton<IEventPublisher, EventPublisher>();

            services.AddSingleton<IPermissionManager, PermissionManager>();
        }
    }
}
