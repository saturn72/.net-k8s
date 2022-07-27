
using EndpointQueryService.Domain;
using Shouldly;
using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xunit;

namespace EndpointQueryService.Tests.IntegrationTests
{
    public class EndpointMeta : IntegrationTestBase
    {

        [Fact]
        public async Task GetEndpointMeta()
        {
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(IntegrationTestsAuthenticationHandler.Schema, "registered");
            string account = "acc",
                endpoint = "ep",
                version = "1",
                expResult = "test";

            GetEndpointByPathResult = new EndpointInfo
            {
                Meta = new Domain.EndpointMeta
                {
                    IdProperty = "id",
                    PublishedOnUtc = DateTime.MaxValue,
                    SearchableBy = new[] { "a", "b", "c" },
                    SubVersion = "1.0.0.1"
                }
            };
            GetEntriesHandler = new[] { new EndpointEntry { Data = expResult } };

            var uri = $"endpoint/_meta/{account}/{endpoint}/{version}";
            var res = await Client.GetStringAsync(uri);

            res.ShouldBe($"[\"{expResult}\"]");
        }
    }
}
