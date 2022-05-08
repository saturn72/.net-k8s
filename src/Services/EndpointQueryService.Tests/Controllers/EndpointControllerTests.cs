using EndpointQueryService.Controllers;
using EndpointQueryService.Domain;
using EndpointQueryService.Services;
using EndpointQueryService.Services.ActivityLog;
using EndpointQueryService.Services.Endpoints;
using EndpointQueryService.Services.Events;
using EndpointQueryService.Services.Rate;
using EndpointQueryService.Services.Security.Permission;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shouldly;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;
using ActionContext = EndpointQueryService.Services.ActionContext;

namespace EndpointQueryService.Tests.Controllers
{
    public class EndpointControllerTests
    {
        [Fact]
        public void ControllerAttributesTests()
        {
            var ac = typeof(EndpointController);
            var allAtts = ac.GetCustomAttributes(true);
            var authAtts = allAtts.Where(a => a.GetType() == typeof(AuthorizeAttribute)).Select(s => s as AuthorizeAttribute);
            authAtts.Count().ShouldBe(1);
            authAtts.First()?.Roles.ShouldBe("subscribed");

            var routeAtts = allAtts.Where(a => a.GetType() == typeof(RouteAttribute)).Select(x => x as RouteAttribute);
            routeAtts.Count().ShouldBe(1);
            routeAtts.First()?.Template.ShouldBe("endpoint");
        }

        [Fact]
        public async Task GetAll_Forbid_OnNullTemplate()
        {
            string an = "account-name",
                ep = "endpoint",
                ver = "v1";

            var es = new Mock<IEndpointService>();

            var ctrl = new EndpointController(es.Object, null, null, null, null);
            var r = await ctrl.GetAllEntries(an, ep, ver);
            r.ShouldBeOfType<ForbidResult>();
        }

        [Fact]
        public async Task GetAll_Forbid_OnUserPermittedForAccount_ReturnsFalse()
        {
            string an = "account-name",
                 ep = "endpoint",
                 t = "temlate",
                ver = "v1"; ;

            var es = new Mock<IEndpointService>();

            var template = new EndpointInfo
            {
            };
            es.Setup(e => e.GetEndpointInfoByPath(It.IsAny<string>())).ReturnsAsync(template);

            var pm = new Mock<IPermissionManager>();
            pm.Setup(p => p.UserIsPermittedForTemplateAction(
                It.IsAny<string>(),
                It.IsAny<GetEntriesContext>())).ReturnsAsync(false);
            var ci = new[]
            {
                new ClaimsIdentity(new[]{new Claim(ClaimTypes.NameIdentifier, "sub-id") }),
            };
            var user = new ClaimsPrincipal(ci);

            var ctrl = new EndpointController(es.Object, pm.Object, null, null, null);
            var ctx = new Mock<HttpContext>();
            ctx.Setup(c => c.User).Returns(user);
            ctrl.ControllerContext.HttpContext = ctx.Object;

            var r = await ctrl.GetAllEntries(an, ep, ver);
            r.ShouldBeOfType<ForbidResult>();
        }
        [Fact]
        public async Task GetAll_Forbid_OnRateManager_ReturnsTrue()
        {
            string an = "account-name",
                 ep = "endpoint",
                ver = "v1";

            var es = new Mock<IEndpointService>();

            var template = new EndpointInfo
            {
            };
            es.Setup(e => e.GetEndpointInfoByPath(It.IsAny<string>())).ReturnsAsync(template);

            var pm = new Mock<IPermissionManager>();
            pm.Setup(p => p.UserIsPermittedForTemplateAction(
                It.IsAny<string>(),
                It.IsAny<GetEntriesContext>())).ReturnsAsync(true);

            var rm = new Mock<IRateManager>();
            rm.Setup(r => r.UserExceededAccessToAccountEndpointVersionAction(It.IsAny<ActionContext>())).ReturnsAsync(true);

            var ci = new[]
{
                new ClaimsIdentity(new[]{new Claim(ClaimTypes.NameIdentifier, "sub-id") }),
            };
            var user = new ClaimsPrincipal(ci);

            var ctx = new Mock<HttpContext>();
            ctx.Setup(c => c.User).Returns(user);
            var ctrl = new EndpointController(es.Object, pm.Object, rm.Object, null, null);

            ctrl.ControllerContext.HttpContext = ctx.Object;
            var r = await ctrl.GetAllEntries(an, ep, ver);
            r.ShouldBeOfType<ForbidResult>();
        }
        [Fact]
        public async Task GetAll_Forbid_GetsEntries_OnError_NoContent_DoesNotPublish()
        {
            string an = "account-name",
                 ep = "endpoint",
                ver = "v1";

            var es = new Mock<IEndpointService>();

            var template = new EndpointInfo
            {
            };
            es.Setup(e => e.GetEndpointInfoByPath(It.IsAny<string>())).ReturnsAsync(template);
            es.Setup(e => e.GetEntriesPage(It.IsAny<GetEntriesContext>())).Callback<GetEntriesContext>(ctx => ctx.Error = "err");

            var pm = new Mock<IPermissionManager>();
            pm.Setup(p => p.UserIsPermittedForTemplateAction(
                It.IsAny<string>(),
                It.IsAny<GetEntriesContext>())).ReturnsAsync(true);

            var rm = new Mock<IRateManager>();
            rm.Setup(r => r.UserExceededAccessToAccountEndpointVersionAction(It.IsAny<ActionContext>())).ReturnsAsync(false);
            var ci = new[]
            {
                new ClaimsIdentity(new[]{new Claim(ClaimTypes.NameIdentifier, "sub-id") }),
            };
            var user = new ClaimsPrincipal(ci);

            var ctx = new Mock<HttpContext>();
            ctx.Setup(c => c.User).Returns(user);
            var clk = new Mock<ISystemClock>();

            var ctrl = new EndpointController(es.Object, pm.Object, rm.Object, null, null);

            ctrl.ControllerContext.HttpContext = ctx.Object;
            var r = await ctrl.GetAllEntries(an, ep, ver);
            r.ShouldBeOfType<OkResult>();
        }
        [Fact]
        public async Task GetAll_Forbid_GetsEntries_Success_OkResult_Publishes()
        {
            string an = "account-name",
                 ep = "endpoint",
                 ver = "v1",
                 data = "data";

            var es = new Mock<IEndpointService>();

            var template = new EndpointInfo
            {
            };
            es.Setup(e => e.GetEndpointInfoByPath(It.IsAny<string>())).ReturnsAsync(template);
            es.Setup(e => e.GetEntriesPage(It.IsAny<GetEntriesContext>())).Callback<GetEntriesContext>(ctx => ctx.Data = data);
            var pm = new Mock<IPermissionManager>();
            pm.Setup(p => p.UserIsPermittedForTemplateAction(
                It.IsAny<string>(),
                It.IsAny<GetEntriesContext>())).ReturnsAsync(true);

            var rm = new Mock<IRateManager>();
            rm.Setup(r => r.UserExceededAccessToAccountEndpointVersionAction(It.IsAny<ActionContext>())).ReturnsAsync(false);
            var ci = new[]
            {
                new ClaimsIdentity(new[]{new Claim(ClaimTypes.NameIdentifier, "sub-id") }),
            };
            var user = new ClaimsPrincipal(ci);

            var ctx = new Mock<HttpContext>();
            ctx.Setup(c => c.User).Returns(user);
            var eb = new Mock<IEventBus>();

            var ctrl = new EndpointController(es.Object, pm.Object, rm.Object, eb.Object, null);

            ctrl.ControllerContext.HttpContext = ctx.Object;
            var r = await ctrl.GetAllEntries(an, ep, ver);
            r.ShouldBeOfType<OkObjectResult>().Value.ShouldBe(data);

            eb.Verify(e => e.Publish(It.IsAny<ApiConsumptionActivityLogRecord>()), Times.Once());
        }
    }
}
