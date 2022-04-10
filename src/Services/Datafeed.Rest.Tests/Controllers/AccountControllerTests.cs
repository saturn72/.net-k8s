
using Datafeed.Rest.Controllers;
using Datafeed.Rest.Domain;
using Datafeed.Rest.Services;
using Datafeed.Rest.Services.Rate;
using Datafeed.Rest.Services.Security.Permission;
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
        public async Task GetAll_Forbid_OnUserPermittedForAccount_ReturnsFalse()
        {
            string an = "account-name",
                ep = "endpoint";

            var account = new Account
            {
                Name = an
            };
            var acc = new Mock<IAccountService>();
            acc.Setup(a => a.GetAccountByName(It.IsAny<string>())).ReturnsAsync(account);

            var pm = new Mock<IPermissionManager>();
            pm.Setup(p => p.UserPermittedForEndpoint(
                It.IsAny<string>(),
                It.IsAny<Account>(),
                It.IsAny<string>())).ReturnsAsync(false);
            var ci = new[]
            {
                new ClaimsIdentity(new[]{new Claim(ClaimTypes.NameIdentifier, "sub-id") }),
            };
            var user = new ClaimsPrincipal(ci);

            var ctx = new Mock<HttpContext>();
            ctx.Setup(c => c.User).Returns(user);
            var ctrl = new AccountController(acc.Object, pm.Object, null, null, null, null);

            ctrl.ControllerContext.HttpContext = ctx.Object;
            var r = await ctrl.GetAll(an, ep);
            r.ShouldBeOfType<ForbidResult>();
        }
        [Fact]
        public async Task GetAll_Forbid_OnRateManager_ReturnsTrue()
        {
            string an = "account-name",
                ep = "endpoint";

            var account = new Account
            {
                Name = an
            };
            var acc = new Mock<IAccountService>();
            acc.Setup(a => a.GetAccountByName(It.IsAny<string>())).ReturnsAsync(account);

            var pm = new Mock<IPermissionManager>();
            pm.Setup(p => p.UserPermittedForEndpoint(
                It.IsAny<string>(), 
                It.IsAny<Account>(),
                It.IsAny<string>())).ReturnsAsync(true);

            var rm = new Mock<IRateManager>();
            rm.Setup(r => r.UserExceededAccessToEndpointAction(
                It.IsAny<string>(), 
                It.IsAny<Account>(), 
                It.IsAny<string>(),
                It.IsAny<string>())).ReturnsAsync(true);
            var ci = new[]
{
                new ClaimsIdentity(new[]{new Claim(ClaimTypes.NameIdentifier, "sub-id") }),
            };
            var user = new ClaimsPrincipal(ci);

            var ctx = new Mock<HttpContext>();
            ctx.Setup(c => c.User).Returns(user);
            var ctrl = new AccountController(acc.Object, pm.Object, rm.Object, null, null, null);

            ctrl.ControllerContext.HttpContext = ctx.Object;
            var r = await ctrl.GetAll(an, ep);
            r.ShouldBeOfType<ForbidResult>();
        }
    }
}
