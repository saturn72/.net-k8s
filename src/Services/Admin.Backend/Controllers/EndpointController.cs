using Admin.Backend.Domain;
using Admin.Backend.Models;
using Admin.Backend.Services;
using Admin.Backend.Services.Endpoint;
using Admin.Backend.Services.Security;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Admin.Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EndpointController : ControllerBase
    {
        private readonly IPermissionManager _permissionManager;
        private readonly IEndpointService _endpoints;
        private readonly IMapper _mapper;
        private readonly ILogger<EndpointController> _logger;

        public EndpointController(
            IPermissionManager permissionManager,
            IEndpointService endpoints,
            IMapper mapper,
            ILogger<EndpointController> logger)
        {
            _permissionManager = permissionManager;
            _endpoints = endpoints;
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
                Model = _mapper.Map<EndpointDomainModel>(model),
            };
            context.UserId = HttpContext.User?.Identity?.Name ?? "anonymous";

            if (!await _permissionManager.UserIsPermittedForEndpointAction(context))
                return Forbid();

            await _endpoints.CreateEndpoint(context);

            return context.IsError ?
                BadRequest(context.UserError) :
                Ok(context.Created);
        }
    }
}