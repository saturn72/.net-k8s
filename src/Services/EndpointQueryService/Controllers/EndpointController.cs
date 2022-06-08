using AnyService.Events;
using EndpointQueryService.Domain;
using EndpointQueryService.Services;
using EndpointQueryService.Services.ActivityLog;
using EndpointQueryService.Services.Endpoints;
using EndpointQueryService.Services.Rate;
using EndpointQueryService.Services.Security.Permission;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EndpointQueryService.Controllers
{
    [ApiController]
    [Route("endpoint/{account}/{endpoint}/{version}")]
    //[Authorize(Roles = "registered")]
    public class EndpointController : ControllerBase
    {
        private readonly IEndpointService _endpointService;
        private readonly IPermissionManager _permissionManager;
        private readonly IRateManager _rateManager;
        private readonly IDomainEventBus _eventBus;
        private readonly ILogger<EndpointController> _logger;

        public EndpointController(
            IEndpointService endpointService,
            IPermissionManager permissionManager,
            IRateManager rateManager,
            IDomainEventBus eventBus,
            ILogger<EndpointController> logger
            )
        {
            _endpointService = endpointService;
            _permissionManager = permissionManager;
            _rateManager = rateManager;
            _eventBus = eventBus;
            _logger = logger;
        }

        [HttpGet]
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
        /// <param name="page">Entries page index (prepopulated on endpoint creation/modification)</param>
        /// <param name="meta">Should include endpoint meta on response</param>
        /// <returns></returns>
        [HttpGet("/page/{page}")]
        public async Task<IActionResult> GetAllEntries(
            string account,
            string endpoint,
            string version,
            int page = 1,
            [FromQuery] bool meta = false)
        {
            var path = $"{account}/{endpoint}/{version}";
            var info = await _endpointService.GetEndpointInfoByPath(path);

            if (info == default) return Forbid();

            //var filter = BuildFilter(info, Request.Query);
            //var ids = BuildIds(id);

            //filter cannot coexist with id request
            //if (ids?.Any() == true && filter != default)
            //    return BadRequest();

            var context = new GetEntriesContext
            {
                Endpoint = info,
                PageNumberRequested = page,
                UserId = User.Subject(),
                //Ids = ids,
                //Filter = filter
            };

            if (!await _permissionManager.UserIsPermittedForEndpointAction(context) ||
                await _rateManager.UserExceededAccessToAccountEndpointVersionAction(context))
                return Forbid();

            await _endpointService.GetEndpointPage(context);

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

            if (!meta)
                return Ok(context.Data);

            var content = new
            {
                context.Endpoint.Meta.TotalPages,
                context.PageSize,
                PageNumber = context.PageNumberReturned,
                context.Data,
            };
            return Ok(content);
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