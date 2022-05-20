using EndpointQueryService.Data.Endpoints;
using EndpointQueryService.Domain;
using EndpointQueryService.Services;
using EndpointQueryService.Services.Endpoints;
using EndpointQueryService.Services.Rate;
using EndpointQueryService.Services.Security.Permission;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace EndpointQueryService.Tests.IntegrationTests
{
    public abstract class IntegrationTestBase
    {
        private readonly WebApplicationFactory<Startup> _factory;
        protected readonly HttpClient Client;
        protected EndpointInfo GetEndpointByPathResult { get; set; }
        protected IEnumerable<EndpointEntry> GetEntriesHandler { get; set; }
        public IntegrationTestBase()
        {
            var endpoints = new Mock<IEndpointRepository>();
            endpoints.Setup(e => e.GetEndpointByPath(It.IsAny<string>())).ReturnsAsync(() => GetEndpointByPathResult);
            endpoints.Setup(e => e.GetEntriesPage(It.IsAny<GetEntriesContext>())).ReturnsAsync(() => GetEntriesHandler);

            _factory = new WebApplicationFactory<Startup>()
               .WithWebHostBuilder(builder =>
               {
                   builder.ConfigureServices(services =>
                   {
                       services.AddSingleton(endpoints.Object);

                       services.AddAuthentication(IntegrationTestsAuthenticationHandler.Schema)
                       .AddScheme<AuthenticationSchemeOptions, IntegrationTestsAuthenticationHandler>(IntegrationTestsAuthenticationHandler.Schema, options => { });
                   });
               });
            Client = _factory.CreateClient();
        }
    }
}
