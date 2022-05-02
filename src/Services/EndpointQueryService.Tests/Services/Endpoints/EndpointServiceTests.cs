using EasyCaching.Core;
using EndpointQueryService.Domain;
using EndpointQueryService.Services;
using EndpointQueryService.Services.Endpoints;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace EndpointQueryService.Tests.Services.Endpoints
{
    public class EndpointServiceTests
    {
        [Fact]
        public async Task GetEntries_ReturnsNoData()
        {
            var log = new Mock<ILogger<EndpointService>>();
            var ch = new Mock<IEasyCachingProvider>();

            var srv = new EndpointService(ch.Object, null, log.Object);
            var ctx = new GetEntriesContext
            {
                Endpoint = new EndpointInfo
                {
                    Account = "acc",
                    Name = "name",
                    Version = "ver"
                }
            };
            await srv.GetEntriesPage(ctx);
            ctx.IsError.ShouldBeTrue();
        }

        [Fact]
        public async Task GetEntries_ReturnsData()
        {
            var log = new Mock<ILogger<EndpointService>>();
            var ch = new Mock<IEasyCachingProvider>();
            var exp = new[] { "aa", "b", "c", "d" };
            var data = new CacheValue<IEnumerable<EndpointEntry>>(exp.Select(s => new EndpointEntry { Data = s }).ToArray(), true);

            ch.Setup(r => r.GetAsync(
                It.IsAny<string>(),
                It.IsAny<Func<Task<IEnumerable<EndpointEntry>>>>(),
                It.IsAny<TimeSpan>())).ReturnsAsync(data);


            var srv = new EndpointService(ch.Object, null, log.Object);
            var ctx = new GetEntriesContext
            {
                Endpoint = new EndpointInfo
                {
                    Account = "acc",
                    Name = "name",
                    Version = "ver"
                }
            };
            await srv.GetEntriesPage(ctx);
            ctx.IsError.ShouldBeFalse();
            var l = ctx.Data.ShouldBeOfType<List<object>>();
            l.ShouldAllBe(x => exp.Contains(x));
        }

        [Fact]
        public async Task GetEndpoint_ReturnsData()
        {
            var log = new Mock<ILogger<EndpointService>>();
            var ch = new Mock<IEasyCachingProvider>();
            var ei = new EndpointInfo();
            var data = new CacheValue<EndpointInfo>(ei, true);

            ch.Setup(r => r.GetAsync(
                It.IsAny<string>(),
                It.IsAny<Func<Task<EndpointInfo>>>(),
                It.IsAny<TimeSpan>())).ReturnsAsync( data);


            var srv = new EndpointService(ch.Object, null, log.Object);
            var res = await srv.GetEndpointInfoByPath("p");
            res.ShouldBe(ei);
        }

        [Fact]
        public async Task GetEndpoint_ReturnsNoData()
        {
            var log = new Mock<ILogger<EndpointService>>();
            var ch = new Mock<IEasyCachingProvider>();
            var ei = new EndpointInfo();
            var data = new CacheValue<EndpointInfo>(ei, false);

            var srv = new EndpointService(ch.Object, null, log.Object);
            var res = await srv.GetEndpointInfoByPath("p");
            res.ShouldBeNull();
        }
    }
}
