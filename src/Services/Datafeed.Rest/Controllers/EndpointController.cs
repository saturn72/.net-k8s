using Datafeed.Rest.Services;
using Datafeed.Rest.Services.ActivityLog;
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
    public class EndpointController : ControllerBase
    {
        private readonly ITemplateService _endpointService;
        private readonly IPermissionManager _permissionManager;
        private readonly IRateManager _rateManager;
        private readonly ISystemClock _systemClock;
        private readonly IEventBus _eventBus;
        public EndpointController(
            ITemplateService endpointService,
            IPermissionManager permissionManager,
            IRateManager rateManager,
            ISystemClock systemClock,
            IEventBus eventBus
            )
        {
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
        public async Task<IActionResult> GetAllEntries(
            string account,
            string endpoint,
            [FromQuery] string template = Consts.Template.Default,
            [FromQuery] string version = Consts.Template.Latest,
            [FromQuery] int offSet = 0,
            [FromQuery] int pageSize = 100)
        {
            var path = $"{account}/{endpoint}/{template}/{version}";
            var t = await _endpointService.GetTemplateByPath(path);

            if (t == default)
                return Forbid();
            var subject = User.Subject();
            var context = new GetAllEntriesContext
            {
                Template = t,
                OffSetRequested = offSet,
                PageSizeRequested = pageSize,
                UserId = subject,
            };

            if (!await _permissionManager.UserIsPermittedForTemplateAction(subject, context) ||
                await _rateManager.UserExceededAccessToAccountEndpointVersionAction(context))
                return Forbid();

            await _endpointService.GetEntries(context);

            if (context.IsError)
                return Ok();

            var a = new ApiConsumptionActivityLogRecord
            {
                Template = t,
                Query = Request?.QueryString.ToString(),
                ConsumedByUserId = context.UserId,
                Version = version,
            };
            _ = _eventBus.Publish(a);
            _ = _rateManager.IncrementAccessToAccountEndpointVersionAction(context);
            return Ok(context.Data);
        }
    }
}