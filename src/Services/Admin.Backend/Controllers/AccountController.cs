using Admin.Backend.Domain;
using Admin.Backend.Models;
using Admin.Backend.Services.Account;
using Admin.Backend.Services.Security;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Admin.Backend.Controllers
{
    [ApiController]
    [Route("admin/account")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IPermissionManager _permissionManager;
        private readonly IMapper _mapper;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            IAccountService accountService,
            IPermissionManager permissionManager,
            IMapper mapper,
            ILogger<AccountController> logger
            )
        {
            _accountService = accountService;
            _permissionManager = permissionManager;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateAccountApiModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var context = new CreateContext<AccountDomainModel>
            {
                Model = _mapper.Map<AccountDomainModel>(model),
                UserId = HttpContext.User?.Identity?.Name ?? "anonymous"
            };

            if (!await _permissionManager.UserIsPermittedForAction(context))
                return Forbid();

            await _accountService.CreateAccount(context);

            return context.IsError ?
                BadRequest(context.UserError) :
                Ok(context.Created);
        }
    }
}