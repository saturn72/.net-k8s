using Admin.Backend.Domain;
using Admin.Backend.Models;
using Admin.Backend.Services.Endpoint;
using Admin.Backend.Services.Security;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Admin.Backend.Controllers
{

    [ApiController]
    [Route("admin/endpoint")]
    public class EndpointController : ControllerBase
    {
        private readonly IPermissionManager _permissionManager;
        private readonly IEndpointService _endpoints;
        private readonly IMapper _mapper;
        private readonly ILogger<EndpointController> _logger;

        public EndpointController(
            IEndpointService endpoints,
            IPermissionManager permissionManager,
            IMapper mapper,
            ILogger<EndpointController> logger)
        {
            _endpoints = endpoints;
            _permissionManager = permissionManager;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateEndpointApiModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var context = new CreateContext<EndpointDomainModel>
            {
                ToCreate = _mapper.Map<EndpointDomainModel>(model),
                UserId = HttpContext.User?.Identity?.Name ?? "anonymous"
            };

            if (!await _permissionManager.UserIsPermittedForAction(context))
                return Forbid();

            await _endpoints.CreateEndpoint(context);

            return context.IsError ?
                BadRequest(context.UserError) :
                Ok(context.Created);
        }
    }
}