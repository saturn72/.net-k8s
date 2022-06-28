using Admin.Backend.Domain;
using Admin.Backend.Models;
using Admin.Backend.Services.Account;
using Admin.Backend.Services.Security;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Admin.Backend.Controllers
{
    [ApiController]
    [Authorize(Roles ="registered")]
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
        public async Task<IActionResult> CreateAccount([FromBody] CreateAccountApiModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var context = new CreateContext<AccountDomainModel>
            {
                ToCreate = _mapper.Map<AccountDomainModel>(model),
                UserId = HttpContext.User?.Identity?.Name ?? "anonymous"
            };
            context.ToCreate.CreatedByUserId = context.UserId;

            if (!await _permissionManager.UserIsPermittedForAction(context))
                return Forbid();

            await _accountService.CreateAccount(context);

            return context.IsError ?
                BadRequest(context.UserError) :
                Ok(context.Created);
        }

        [HttpGet]
        public async Task<IActionResult> GetAccountBySubjectId()
        {
            var subjectId = HttpContext.User?.Identity?.Name;
            var context = new ReadByIndexContext<AccountDomainModel>
            {
                Index = subjectId,
                IndexName = Defaults.Account.IndexNames.SubjectId,
                UserId = subjectId,
            };

            if (!await _permissionManager.UserIsPermittedForAction(context))
                return Forbid();

            await _accountService.GetAccountBySubjectId(context);

            return context.IsError ?
                BadRequest(context.UserError) :
                Ok(context.Read);
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetAccountByName(string name)
        {
            var context = new ReadByIndexContext<AccountDomainModel>
            {
                Index = name,
                IndexName = Defaults.Account.IndexNames.Name,
                UserId = HttpContext.User?.Identity?.Name,
            };

            if (!await _permissionManager.UserIsPermittedForAction(context))
                return Forbid();

            await _accountService.GetAccountByName(context);

            return context.IsError ?
                BadRequest(context.UserError) :
                Ok(context.Read);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAccount([FromBody] UpdateAccountApiModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var context = new UpdateContext<AccountDomainModel>
            {
                ToUpdate = _mapper.Map<AccountDomainModel>(model),
                UserId = HttpContext.User?.Identity?.Name ?? "anonymous"
            };

            if (!await _permissionManager.UserIsPermittedForAction(context))
                return Forbid();

            await _accountService.UpdateAccount(context);

            return context.IsError ?
                BadRequest(context.UserError) :
                Ok(context.After);
        }

        [HttpGet("{accountId}")]
        public async Task<IActionResult> GetDeleteAccountToken(string accountId)
        {
            var context = new DeleteContext<AccountDomainModel>
            {
                IdToDelete = accountId,
                UserId = HttpContext.User?.Identity?.Name,
            };
            if (!await _permissionManager.UserIsPermittedForAction(context))
                return Forbid();

            var token = await _accountService.GetAccountDeletionToken(context.UserId, accountId);
            return Ok(token);
        }

        [HttpDelete("{accountId}/{token}")]
        public async Task<IActionResult> DeleteAccount(string accountId, string token)
        {
            var context = new DeleteContext<AccountDomainModel>
            {
                IdToDelete = accountId,
                DeleteToken = token,
                UserId = HttpContext.User?.Identity?.Name,
            };

            if (!await _permissionManager.UserIsPermittedForAction(context))
                return Forbid();

            await _accountService.DeleteAccountById(context);

            return context.IsError ?
                BadRequest(context.UserError) :
                Ok(context.Deleted.Id);
        }
    }
}