using EndpointQueryService.Domain;
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
    //[Authorize(Roles = "registered")]
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

        [HttpGet("_meta/{account}/{endpoint}/{version}")]
        public async Task<IActionResult> GetEndpointMeta(
            string account,
            string endpoint,
            string version)
        {
            var path = $"{account}/{endpoint}/{version}";
            var e = await _endpointService.GetEndpointInfoByPath(path);

            if (e == default) return Forbid();

            var context = new MetaContext
            {
                Endpoint = e,
                UserId = User.Subject()
            };

            if (!await _permissionManager.UserIsPermittedForEndpointAction(context) ||
                await _rateManager.UserExceededAccessToAccountEndpointVersionAction(context))
                return Forbid();

            var a = new ApiConsumptionActivityLogRecord
            {
                Endpoint = e,
                Query = Request?.QueryString.ToString(),
                ConsumedByUserId = context.UserId,
                Version = version,
            };
            _ = _eventBus.Publish(a);
            return Ok(e.Meta);
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
        [HttpGet("{account}/{endpoint}/{version}")]
        public async Task<IActionResult> GetAllEntries(
            string account,
            string endpoint,
            string version,
            [FromQuery] IEnumerable<string>? id,
            [FromQuery] int offSet = 0,
            [FromQuery] int pageSize = 100)
        {
            var path = $"{account}/{endpoint}/{version}";
            var info = await _endpointService.GetEndpointInfoByPath(path);

            if (info == default) return Forbid();

            var filter = BuildFilter(info, Request.Query);
            var ids = BuildIds(id);

            //filter cannot coexist with id request
            if (ids?.Any() == true && filter != default)
                return BadRequest();

            var context = new GetEntriesContext
            {
                Endpoint = info,
                OffSetRequested = offSet,
                PageSizeRequested = pageSize,
                UserId = User.Subject(),
                Ids = ids,
                Filter = filter
            };

            if (!await _permissionManager.UserIsPermittedForEndpointAction(context) ||
                await _rateManager.UserExceededAccessToAccountEndpointVersionAction(context))
                return Forbid();

            await _endpointService.GetEntries(context);

            if (context.IsError)
                return Ok();

            var a = new ApiConsumptionActivityLogRecord
            {
                Endpoint = info,
                Query = Request?.QueryString.ToString(),
                ConsumedByUserId = context.UserId,
                Version = version,
            };
            _ = _eventBus.Publish(a);
            _ = _rateManager.IncrementAccessToAccountEndpointVersionAction(context);
            return Ok(context.Data);
        }

        private static IEnumerable<string>? BuildIds(IEnumerable<string>? requestedIds)
        {
            if (requestedIds == default) return default;

            var res = new List<string>();
            foreach (var rid in requestedIds)
            {
                var c = rid.Trim().ToLowerInvariant();
                if (!c.HasValue() || res.Contains(c))
                    continue;

                res.Add(c);
            }
            return res;
        }

        private static IEnumerable<KeyValuePair<string, string>> BuildFilter(
            EndpointInfo info,
            IQueryCollection query)
        {
            var q = query.ToList();
            var l = new List<KeyValuePair<string, string>>();
            var sb = info.Meta.SearchableBy;
            for (int i = 0; i < query.Count; i++)
            {
                var c = q.ElementAt(i);
                if (!sb.Contains(c.Key)) continue;
                l.Add(new KeyValuePair<string, string>(c.Key, c.Value));
            }
            return l;
        }
    }
}