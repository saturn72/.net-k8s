﻿using Admin.Backend.Domain;
using Admin.Backend.Services;
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
            services.AddSingleton<IEndpointService, EndpointService>();
            services.AddSingleton<IValidator<CreateContext<EndpointDomainModel>>, EndpointValidator>();


            services.AddSingleton<IDatasourceService, DatasourceService>();
            services.AddSingleton<IValidator<CreateContext<DatasourceDomainModel>>, DatasourceValidator>();

            services.AddSingleton<IDomainEventBus, DefaultDomainEventsBus>();
            services.AddSingleton<ISubscriptionManager<DomainEvent>, DefaultSubscriptionManager<DomainEvent>>();
            services.AddSingleton<ISubscriptionManager<IntegrationEvent>, DefaultSubscriptionManager<IntegrationEvent>>();

            services.AddSingleton<IPermissionManager, PermissionManager>();
        }
    }
}
