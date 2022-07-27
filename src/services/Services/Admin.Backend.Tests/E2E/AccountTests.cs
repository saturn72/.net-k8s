using Microsoft.AspNetCore.Mvc.Testing;
using Shouldly;
using System.Net;
using System.Net.Http.Headers;

namespace Admin.Backend.Tests.E2E
{
    public class AccountTests : E2ETestBase
    {
        private const string URI = "admin/account";
        public AccountTests(WebApplicationFactory<Program> factory) : base(factory)
        {
        }

        [Fact]
        public async Task Account_CRUD()
        {
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "registered");

            var acc = new
            {
                Name = "saturn72"
            };
            var res = await Client.PostAsJsonAsync(URI, acc);
            res.StatusCode.ShouldBe(HttpStatusCode.OK);

        }
    }
}