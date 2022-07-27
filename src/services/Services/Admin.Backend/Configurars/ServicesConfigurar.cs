using Admin.Backend.Domain;
using Admin.Backend.Services;
using Admin.Backend.Services.Account;
using Admin.Backend.Services.Datasource;
using Admin.Backend.Services.Endpoint;
using Admin.Backend.Services.Security;
using AnyService.Events;

namespace Admin.Backend.Configurars
{
    public class ServicesConfigurar
    {
        public void Configure(IServiceCollection services)
        {
            services.AddSingleton<IAccountService, AccountService>();
            services.AddSingleton<IValidator<CreateContext<AccountDomainModel>>, AccountValidator>();
            services.AddSingleton<IValidator<ReadByIdContext<AccountDomainModel>>, AccountValidator>();
            services.AddSingleton<IValidator<ReadByIndexContext<AccountDomainModel>>, AccountValidator>();
            services.AddSingleton<IValidator<UpdateContext<AccountDomainModel>>, AccountValidator>();
            services.AddSingleton<IValidator<DeleteContext<AccountDomainModel>>, AccountValidator>();

            services.AddSingleton<IDatasourceService, DatasourceService>();
            services.AddSingleton<IValidator<CreateContext<DatasourceDomainModel>>, DatasourceValidator>();
            services.AddSingleton<IValidator<ReadByIdContext<DatasourceDomainModel>>, DatasourceValidator>();
            services.AddSingleton<IValidator<PaginationContext<DatasourceDomainModel>>, DatasourceValidator>();

            services.AddSingleton<IEndpointService, EndpointService>();
            services.AddSingleton<IValidator<CreateContext<EndpointDomainModel>>, EndpointValidator>();

            services.AddSingleton<IDomainEventBus, DefaultDomainEventsBus>();
            services.AddSingleton<ISubscriptionManager<DomainEvent>, DefaultSubscriptionManager<DomainEvent>>();
            services.AddSingleton<ISubscriptionManager<IntegrationEvent>, DefaultSubscriptionManager<IntegrationEvent>>();

            services.AddSingleton<IPermissionManager, PermissionManager>();
        }
    }
}
