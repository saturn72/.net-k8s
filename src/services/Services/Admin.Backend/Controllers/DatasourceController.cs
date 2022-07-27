using Admin.Backend.Domain;
using Admin.Backend.Models;
using Admin.Backend.Services.Datasource;
using Admin.Backend.Services.Security;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Admin.Backend.Controllers
{
    [ApiController]
    [Route("admin/datasource")]
    public class DatasourceController : ControllerBase
    {
        private readonly IDatasourceService _datasourceService;
        private readonly IPermissionManager _permissionManager;
        private readonly IMapper _mapper;
        private readonly ILogger<DatasourceController> _logger;

        public DatasourceController(
            IDatasourceService datasourceService,
            IPermissionManager permissionManager,
            IMapper mapper,
            ILogger<DatasourceController> logger
            )
        {
            _datasourceService = datasourceService;
            _permissionManager = permissionManager;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateDatasourceApiModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var context = new CreateContext<DatasourceDomainModel>
            {
                ToCreate = _mapper.Map<DatasourceDomainModel>(model),
                UserId = HttpContext.User?.Identity?.Name ?? "anonymous",
            };
            context.ToCreate.CreatedByUserId = context.UserId;

            if (!await _permissionManager.UserIsPermittedForAction(context))
                return Forbid();

            await _datasourceService.CreateDatasource(context);

            return context.IsError ?
                BadRequest(context.UserError) :
                Ok(context.Created);
        }

        [HttpGet]
        public async Task<IActionResult> GetDatasourceByUser(
            [FromQuery] uint pageSize = 100,
            [FromQuery] uint offset = 0,
            [FromQuery] string? orderBy = "id",
            [FromQuery] bool asc = true
            )
        {
            if (pageSize > Defaults.Pagination.MaxPageSize)
                pageSize = Defaults.Pagination.MaxPageSize;

            var userId = HttpContext.User?.Identity?.Name ?? "anonymous";
            var context = new PaginationContext<DatasourceDomainModel>
            {
                UserId = userId,
                PageSize = pageSize,
                Offset = offset,
                OrderBy = orderBy,
                IsAsc = asc,
            };

            if (!await _permissionManager.UserIsPermittedForAction(context))
                return Forbid();

            await _datasourceService.GetDatasources(context);

            return context.IsError ?
                BadRequest(context.UserError) :
                Ok(context.Data.ToArray());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDatasourceById(string id)
        {
            var context = new ReadByIdContext<DatasourceDomainModel>
            {
                Id = id,
                UserId = HttpContext.User?.Identity?.Name ?? "anonymous",
            };

            if (!await _permissionManager.UserIsPermittedForAction(context))
                return Forbid();

            await _datasourceService.GetDatasourceById(context);

            return context.IsError ?
                BadRequest(context.UserError) :
                Ok(context.Value);
        }
    }
}