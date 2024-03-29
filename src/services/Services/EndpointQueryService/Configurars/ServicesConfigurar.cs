﻿using AnyService.Events;
using EndpointQueryService.Services.Endpoints;
using EndpointQueryService.Services.Rate;
using EndpointQueryService.Services.Security.Permission;

namespace EndpointQueryService.Configurars
{
    public class ServicesConfigurar
    {
        public void Configure(IServiceCollection services)
        {
            services.AddSingleton<IEndpointService, EndpointService>();

            services.AddSingleton<IPermissionManager, PermissionManager>();
            services.AddSingleton<IRateManager, RateManager>();
            services.AddSingleton<IRateRepository, DapperRateRepository>();

            services.AddSingleton<IDomainEventBus, DefaultDomainEventsBus>();
        }
    }
}
