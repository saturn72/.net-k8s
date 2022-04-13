using Datafeed.Rest.Services;
using Datafeed.Rest.Services.ActivityLog;
using Datafeed.Rest.Services.Endpoints;
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
        private readonly IEndpointService _endpointService;
        private readonly IPermissionManager _permissionManager;
        private readonly IRateManager _rateManager;
        private readonly ISystemClock _systemClock;
        private readonly IEventBus _eventBus;
        public AccountController(
            IAccountService accountService,
            IEndpointService endpointService,
            IPermissionManager permissionManager,
            IRateManager rateManager,
            ISystemClock systemClock,
            IEventBus eventBus
            )
        {
            _accountService = accountService;
            _endpointService = endpointService;
            _permissionManager = permissionManager;
            _rateManager = rateManager;
            _systemClock = systemClock;
            _eventBus = eventBus;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="account">Account name</param>
        /// <param name="endpoint">Endpoint name</param>
        /// <param name="version">Endpoint version name</param>
        /// <param name="offSet">Offset value. normalized to 100 intervals</param>
        /// <param name="pageSize">Page size value. normalized to 100 intervals. max value = 1000</param>
        /// <returns></returns>
        [HttpGet("{accountName}/{endpoint}")]
        public async Task<IActionResult> GetAll(
            string account,
            string endpoint,
            [FromQuery] string version = Consts.AccountEndpointVersion.LatestAlias,
            [FromQuery] int offSet = 0,
            [FromQuery] int pageSize = 100)
        {
            var acc = await _accountService.GetAccountByName(account);
            if (acc == default)
                return Forbid();

            var ae = await _endpointService.GetEndpointByName(acc, endpoint);
            if (ae == default)
                return Forbid();

            if (version != Consts.AccountEndpointVersion.LatestAlias &&
                !ae.VersionNames.Contains(version))
                return Forbid();

            string subject;
            if (!await _permissionManager.UserIsPermittedForEndpoint(subject = User.Subject(), ae) ||
                await _rateManager.UserExceededAccessToAccountEndpointVersionAction(subject, ae, version, Consts.AccountEndpointVersion.Actions.GetAll))
                return Forbid();

            normalizeOffset();
            normalizePageSize();

            var context = new GetEntriesContext
            {
                Account = acc,
                Endpoint = ae,
                Version = version,
                OffSet = offSet,
                PageSize = pageSize,
            };
            await _endpointService.GetEntries(context);

            if (context.IsError)
                return NoContent();

            var a = new ApiConsumptionActivityLogRecord
            {
                Accout = acc,
                ConsumedOnUtc = _systemClock.UtcNow,
                Endpoint = ae,
                Query = Request?.QueryString.ToString(),
                ConsumedByUserId = subject,
                Version = version,
            };
            _ = _eventBus.Publish(a);
            return Ok(context.Data);

            void normalizePageSize()
            {
                if (pageSize <= 100)
                {
                    pageSize = 100;
                }
                else if (pageSize > 1000)
                {
                    pageSize = 1000;
                }
                else
                {
                    pageSize = pageSize / 100 * 100;
                }
            }
            void normalizeOffset()
            {
                if (offSet != 0)
                    offSet = offSet / 100 * 100;
            }
        }
    }
}