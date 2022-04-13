using Datafeed.Rest.Controllers;
using Datafeed.Rest.Domain;
using Datafeed.Rest.Services;
using Datafeed.Rest.Services.ActivityLog;
using Datafeed.Rest.Services.Endpoints;
using Datafeed.Rest.Services.Events;
using Datafeed.Rest.Services.Rate;
using Datafeed.Rest.Services.Security.Permission;
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

namespace Datafeed.Rest.Tests.Controllers
{
    public class AccountControllerTests
    {
        [Fact]
        public void ControllerAttributesTests()
        {
            var ac = typeof(AccountController);
            var allAtts = ac.GetCustomAttributes(true);
            var authAtts = allAtts.Where(a => a.GetType() == typeof(AuthorizeAttribute)).Select(s => s as AuthorizeAttribute);
            authAtts.Count().ShouldBe(1);
            authAtts.First().Roles.ShouldBe("subscribed");

            var routeAtts = allAtts.Where(a => a.GetType() == typeof(RouteAttribute)).Select(x => x as RouteAttribute);
            routeAtts.Count().ShouldBe(1);
            routeAtts.First().Template.ShouldBe("");
        }
        [Fact]
        public async Task GetAll_Forbid_OnNullAccount()
        {
            string an = "account-name",
                ep = "endpoint";

            var acc = new Mock<IAccountService>();

            var ctrl = new AccountController(acc.Object, null, null, null, null, null);
            var r = await ctrl.GetAll(an, ep);
            r.ShouldBeOfType<ForbidResult>();
        }
        [Fact]
        public async Task GetAll_Forbid_OnNullEndpoint()
        {
            string an = "account-name",
                ep = "endpoint";

            var acSrv = new Mock<IAccountService>();
            var acc = new Account();
            acSrv.Setup(a => a.GetAccountByName(It.IsAny<string>())).ReturnsAsync(acc);
            var aeSrv = new Mock<IEndpointService>();

            var ctrl = new AccountController(acSrv.Object, aeSrv.Object, null, null, null, null);
            var r = await ctrl.GetAll(an, ep);
            r.ShouldBeOfType<ForbidResult>();
        }
        [Fact]
        public async Task GetAll_Forbid_OnVersionNameNotExists()
        {
            string an = "account-name",
                ep = "endpoint",
                ver = "ver";

            var acSrv = new Mock<IAccountService>();
            var acc = new Account();
            acSrv.Setup(a => a.GetAccountByName(It.IsAny<string>())).ReturnsAsync(acc);
            var aeSrv = new Mock<IEndpointService>();
            var en = new AccountEndpoint
            {
                VersionNames = new[] { "v1" },
            };
            aeSrv.Setup(e => e.GetEndpointByName(It.IsAny<Account>(), It.IsAny<string>())).ReturnsAsync(en);
            var ctrl = new AccountController(acSrv.Object, aeSrv.Object, null, null, null, null);
            var ci = new[]
           {
                new ClaimsIdentity(new[]{new Claim(ClaimTypes.NameIdentifier, "sub-id") }),
            };
            var user = new ClaimsPrincipal(ci);

            var ctx = new Mock<HttpContext>();
            ctx.Setup(c => c.User).Returns(user);
            ctrl.ControllerContext.HttpContext = ctx.Object;
            
            var r = await ctrl.GetAll(an, ep, ver);
            r.ShouldBeOfType<ForbidResult>();
        }

        [Fact]
        public async Task GetAll_Forbid_OnUserPermittedForAccount_ReturnsFalse()
        {
            string an = "account-name",
                ep = "endpoint",
                ver = "ver";

            var account = new Account
            {
                Name = an
            };
            var acc = new Mock<IAccountService>();
            acc.Setup(a => a.GetAccountByName(It.IsAny<string>())).ReturnsAsync(account);
            var aeSrv = new Mock<IEndpointService>();
            var en = new AccountEndpoint
            {
                VersionNames = new[] { ver },
            };
            aeSrv.Setup(e => e.GetEndpointByName(It.IsAny<Account>(), It.IsAny<string>())).ReturnsAsync(en);

            var pm = new Mock<IPermissionManager>();
            pm.Setup(p => p.UserIsPermittedForEndpoint(
                It.IsAny<string>(),
                It.IsAny<AccountEndpoint>())).ReturnsAsync(false);
            var ci = new[]
            {
                new ClaimsIdentity(new[]{new Claim(ClaimTypes.NameIdentifier, "sub-id") }),
            };
            var user = new ClaimsPrincipal(ci);

            var ctrl = new AccountController(acc.Object, aeSrv.Object, pm.Object, null, null, null);
            var ctx = new Mock<HttpContext>();
            ctx.Setup(c => c.User).Returns(user);
            ctrl.ControllerContext.HttpContext = ctx.Object;

            var r = await ctrl.GetAll(an, ep, ver);
            r.ShouldBeOfType<ForbidResult>();
        }
        [Fact]
        public async Task GetAll_Forbid_OnRateManager_ReturnsTrue()
        {
            string an = "account-name",
                ep = "endpoint",
                ver = "ver";

            var account = new Account
            {
                Name = an
            };
            var acc = new Mock<IAccountService>();
            acc.Setup(a => a.GetAccountByName(It.IsAny<string>())).ReturnsAsync(account);
            var aeSrv = new Mock<IEndpointService>();
            var en = new AccountEndpoint
            {
                VersionNames = new[] { ver },
            };
            aeSrv.Setup(e => e.GetEndpointByName(It.IsAny<Account>(), It.IsAny<string>())).ReturnsAsync(en);
            var pm = new Mock<IPermissionManager>();
            pm.Setup(p => p.UserIsPermittedForEndpoint(
                It.IsAny<string>(),
                It.IsAny<AccountEndpoint>())).ReturnsAsync(true);

            var rm = new Mock<IRateManager>();
            rm.Setup(r => r.UserExceededAccessToAccountEndpointVersionAction(
                It.IsAny<string>(),
                It.IsAny<AccountEndpoint>(),
                It.IsAny<string>(),
                It.IsAny<string>())).ReturnsAsync(true);
            var ci = new[]
{
                new ClaimsIdentity(new[]{new Claim(ClaimTypes.NameIdentifier, "sub-id") }),
            };
            var user = new ClaimsPrincipal(ci);

            var ctx = new Mock<HttpContext>();
            ctx.Setup(c => c.User).Returns(user);
            var ctrl = new AccountController(acc.Object, aeSrv.Object, pm.Object, rm.Object, null, null);

            ctrl.ControllerContext.HttpContext = ctx.Object;
            var r = await ctrl.GetAll(an, ep, ver);
            r.ShouldBeOfType<ForbidResult>();
        }
        [Fact]
        public async Task GetAll_Forbid_GetsEntries_OnError_NoContent_DoesNotPublish()
        {
            string an = "account-name",
                ep = "endpoint",
                ver = "ver";

            var account = new Account
            {
                Name = an
            };
            var acc = new Mock<IAccountService>();
            acc.Setup(a => a.GetAccountByName(It.IsAny<string>())).ReturnsAsync(account);
            var aeSrv = new Mock<IEndpointService>();
            var en = new AccountEndpoint
            {
                VersionNames = new[] { ver },
            };
            aeSrv.Setup(e => e.GetEndpointByName(It.IsAny<Account>(), It.IsAny<string>())).ReturnsAsync(en);
            aeSrv.Setup(e => e.GetEntries(It.IsAny<GetEntriesContext>())).Callback<GetEntriesContext>(ctx => ctx.Error = "err");

            var pm = new Mock<IPermissionManager>();
            pm.Setup(p => p.UserIsPermittedForEndpoint(
                It.IsAny<string>(),
                It.IsAny<AccountEndpoint>())).ReturnsAsync(true);

            var rm = new Mock<IRateManager>();
            rm.Setup(r => r.UserExceededAccessToAccountEndpointVersionAction(
                It.IsAny<string>(),
                It.IsAny<AccountEndpoint>(),
                It.IsAny<string>(),
                It.IsAny<string>())).ReturnsAsync(false);
            var ci = new[]
            {
                new ClaimsIdentity(new[]{new Claim(ClaimTypes.NameIdentifier, "sub-id") }),
            };
            var user = new ClaimsPrincipal(ci);

            var ctx = new Mock<HttpContext>();
            ctx.Setup(c => c.User).Returns(user);
            var clk = new Mock<ISystemClock>();

            var ctrl = new AccountController(acc.Object, aeSrv.Object, pm.Object, rm.Object, clk.Object, null);

            ctrl.ControllerContext.HttpContext = ctx.Object;
            var r = await ctrl.GetAll(an, ep, ver);
            r.ShouldBeOfType<NoContentResult>();
        }
        [Fact]
        public async Task GetAll_Forbid_GetsEntries_Success_OkResult_Publishes()
        {
            string an = "account-name",
                ep = "endpoint",
                ver = "ver",
                data = "data";

            var account = new Account
            {
                Name = an
            };
            var acc = new Mock<IAccountService>();
            acc.Setup(a => a.GetAccountByName(It.IsAny<string>())).ReturnsAsync(account);
            var aeSrv = new Mock<IEndpointService>();
            var en = new AccountEndpoint
            {
                VersionNames = new[] { ver },
            };
            aeSrv.Setup(e => e.GetEndpointByName(It.IsAny<Account>(), It.IsAny<string>())).ReturnsAsync(en);
            aeSrv.Setup(e => e.GetEntries(It.IsAny<GetEntriesContext>())).Callback<GetEntriesContext>(ctx => ctx.Data= data);

            var pm = new Mock<IPermissionManager>();
            pm.Setup(p => p.UserIsPermittedForEndpoint(
                It.IsAny<string>(),
                It.IsAny<AccountEndpoint>())).ReturnsAsync(true);

            var rm = new Mock<IRateManager>();
            rm.Setup(r => r.UserExceededAccessToAccountEndpointVersionAction(
                It.IsAny<string>(),
                It.IsAny<AccountEndpoint>(),
                It.IsAny<string>(),
                It.IsAny<string>())).ReturnsAsync(false);
            var ci = new[]
            {
                new ClaimsIdentity(new[]{new Claim(ClaimTypes.NameIdentifier, "sub-id") }),
            };
            var user = new ClaimsPrincipal(ci);

            var ctx = new Mock<HttpContext>();
            ctx.Setup(c => c.User).Returns(user);
            var clk = new Mock<ISystemClock>();
            var eb = new Mock<IEventBus>();

            var ctrl = new AccountController(acc.Object, aeSrv.Object, pm.Object, rm.Object, clk.Object, eb.Object);

            ctrl.ControllerContext.HttpContext = ctx.Object;
            var r = await ctrl.GetAll(an, ep, ver);
            r.ShouldBeOfType<OkObjectResult>().Value.ShouldBe(data);

            eb.Verify(e => e.Publish(It.IsAny<ApiConsumptionActivityLogRecord>()), Times.Once());
        }
    }
}
