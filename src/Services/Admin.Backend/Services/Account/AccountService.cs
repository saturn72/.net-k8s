using Admin.Backend.Domain;
using AnyService.Events;

namespace Admin.Backend.Services.Account
{
    public class AccountService : IAccountService
    {
        private readonly IValidator<CreateContext<AccountDomainModel>> _validator;
        private readonly IAccountStore _store;
        private readonly IDomainEventBus _events;
        private readonly ILogger<IAccountService> _logger;

        public AccountService(
            IValidator<CreateContext<AccountDomainModel>> validator,
            IAccountStore store,
            IDomainEventBus events,
            ILogger<IAccountService> logger)
        {
            _validator = validator;
            _store = store;
            _events = events;
            _logger = logger;
        }
        public async Task CreateAccount(CreateContext<AccountDomainModel> context)
        {
            if (!await _validator.IsValidFor(context) || context.IsError)
            {
                _logger.LogInformation(context.Error);
                return;
            }
            await _store.Create(context);
            if (context.IsError)
            {
                _logger.LogError(context.Error);
                return;
            }
            _ = _events.Publish(EventKeys.Account.Created, context);
        }
    }
}
