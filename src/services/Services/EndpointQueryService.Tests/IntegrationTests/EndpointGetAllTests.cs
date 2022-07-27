
using EndpointQueryService.Domain;
using Shouldly;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xunit;

namespace EndpointQueryService.Tests.IntegrationTests
{
    public class EndpointGetAllTests : IntegrationTestBase
    {
        [Fact]
        public async Task GetEndpointPage_UnauthorizedUser()
        {
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(IntegrationTestsAuthenticationHandler.Schema, "not-registered");

            string account = "acc",
                endpoint = "ep",
                version = "1";

            var uri = $"endpoint/{account}/{endpoint}/{version}";
            var res = await Client.GetAsync(uri);

            res.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task GetEndpointPage_GetPage()
        {
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(IntegrationTestsAuthenticationHandler.Schema, "registered");
            string account = "acc",
                endpoint = "ep",
                version = "1",
                expResult = "test";

            GetEndpointByPathResult = new EndpointInfo
            {
                Meta = new()
                {
                    Account = account,
                    Name = endpoint,
                    Version = version,
                }
            };
            GetEntriesHandler = new[] { new EndpointEntry { Data = expResult } };

            var uri = $"endpoint/{account}/{endpoint}/{version}";
            var res = await Client.GetStringAsync(uri);

            res.ShouldBe($"[\"{expResult}\"]");
        }
    }
}
