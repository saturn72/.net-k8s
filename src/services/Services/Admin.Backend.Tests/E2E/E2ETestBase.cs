using Microsoft.AspNetCore.Mvc.Testing;

namespace Admin.Backend.Tests.E2E
{
    public class E2ETestBase : IClassFixture<WebApplicationFactory<Program>>
    {
        protected HttpClient Client;
        private readonly WebApplicationFactory<Program> _factory;

        public E2ETestBase(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            //var application = new WebApplicationFactory<Program>()
            //.WithWebHostBuilder(builder =>
            //{
            //});

            Client = _factory.CreateClient();
        }
    }
}
