using Datafeed.Rest.Services;
using Datafeed.Rest.Services.ActivityLog;
using Datafeed.Rest.Services.Datafeed;
using Datafeed.Rest.Services.Events;
using Datafeed.Rest.Services.Rate;
using Datafeed.Rest.Services.Security.Permission;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Datafeed.Rest.Controllers
{
    [ApiController]
    [Route("")]
    [Authorize(Roles = "subscribed")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IPermissionManager _permissionManager;
        private readonly IRateManager _rateManager;
        private readonly IDatafeedService _datafeedService;
        private readonly ISystemClock _systemClock;
        private readonly IEventBus _eventBus;

        private const string Latest = "latest";
        public AccountController(
            IAccountService accountService,
            IPermissionManager permissionManager,
            IRateManager rateManager,
            IDatafeedService datafeedService,
            ISystemClock systemClock,
            IEventBus eventBus
            )
        {
            _accountService = accountService;
            _permissionManager = permissionManager;
            _rateManager = rateManager;
            _datafeedService = datafeedService;
            _systemClock = systemClock;
            _eventBus = eventBus;
        }

        [HttpGet("{accountName}/{endpoint}")]
        public async Task<IActionResult> GetAll(
            string accountName,
            string endpoint,
            [FromQuery] string version = Latest,
            [FromQuery] int offSet = 0,
            [FromQuery] int pageSize = 100)
        {
            var account = await _accountService.GetAccountByName(accountName);
            string subject;

            if (account == default ||
                !await _permissionManager.UserPermittedForEndpoint(subject = User.Subject(), account, endpoint) ||
                await _rateManager.UserExceededAccessToEndpointAction(subject, account, endpoint, "get"))
                return Forbid();

            var context = new GetAllContext
            {
                Account = account,
                Endpoint = endpoint,
                OffSet = offSet,
                PageSize = pageSize,
                Version = version,
            };
            await _datafeedService.GetAll(context);

            var a = new ApiConsumptionActivityLogRecord
            {
                Accout = account,
                ConsumedOnUtc = _systemClock.UtcNow,
                Endpoint = endpoint,
                Query = Request.QueryString.ToString(),
                UserId = subject,
                Version = version,
            };
            _ = _eventBus.Publish(a);

            return Ok(context.Data);
        }
    }
}