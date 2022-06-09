using EndpointQueryService.Services;
using Shouldly;
using Xunit;

namespace EndpointQueryService.Tests.Services.Endpoints
{
    public class ConstTests
    {
        [Fact]
        public void Const_Endpoint_Actions_AllFields()
        {
            Consts.Endpoint.Actions.PageRead.ShouldBe("get-all");
            Consts.Endpoint.Actions.Meta.ShouldBe("meta");
        }
    }
}
