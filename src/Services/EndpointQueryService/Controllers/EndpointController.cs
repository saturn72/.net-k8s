using EndpointQueryService.Services;
using EndpointQueryService.Services.ActivityLog;
using EndpointQueryService.Services.Endpoints;
using EndpointQueryService.Services.Events;
using EndpointQueryService.Services.Rate;
using EndpointQueryService.Services.Security.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EndpointQueryService.Controllers
{
    [ApiController]
    [Route("endpoint")]
    [Authorize(Roles = "subscribed")]
    public class EndpointController : ControllerBase
    {
        private readonly IEndpointService _endpointService;
        private readonly IPermissionManager _permissionManager;
        private readonly IRateManager _rateManager;
        private readonly IEventBus _eventBus;
        private readonly ILogger<EndpointController> _logger;

        public EndpointController(
            IEndpointService endpointService,
            IPermissionManager permissionManager,
            IRateManager rateManager,
            IEventBus eventBus,
            ILogger<EndpointController> logger
            )
        {
            _endpointService = endpointService;
            _permissionManager = permissionManager;
            _rateManager = rateManager;
            _eventBus = eventBus;
            _logger = logger;
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
        [HttpGet("{account}/{endpoint}")]
        public async Task<IActionResult> GetAllEntries(
            string account,
            string endpoint,
            [FromQuery] string? version = Consts.Endpoint.LatestVersion,
            [FromQuery] int offSet = 0,
            [FromQuery] int pageSize = 100)
        {
            var path = $"{account}/{endpoint}/{version}";
            var t = await _endpointService.GetEndpointInfoByPath(path);

            if (t == default)
                return Forbid();
            var subject = User.Subject();
            var context = new GetEntriesContext
            {
                Endpoint = t,
                OffSetRequested = offSet,
                PageSizeRequested = pageSize,
                UserId = subject,
            };

            if (!await _permissionManager.UserIsPermittedForTemplateAction(subject, context) ||
                await _rateManager.UserExceededAccessToAccountEndpointVersionAction(context))
                return Forbid();

            await _endpointService.GetEntriesPage(context);

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