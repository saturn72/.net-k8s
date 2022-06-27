using Admin.Backend.Domain;
using AnyService.Events;
using EasyCaching.Core;

namespace Admin.Backend.Services.Account
{
    public class AccountService : IAccountService
    {
        private readonly IValidator<CreateContext<AccountDomainModel>> _createValidator;
        private readonly IValidator<ReadByIndexContext<AccountDomainModel>> _readByIndexValidator;
        private readonly IValidator<UpdateContext<AccountDomainModel>> _updateValidator;
        private readonly IValidator<DeleteContext<AccountDomainModel>> _deleteValidator;
        private readonly IAccountStore _store;
        private readonly IEasyCachingProvider _cache;
        private readonly IDomainEventBus _events;
        private readonly ILogger<IAccountService> _logger;

        public AccountService(
            IValidator<CreateContext<AccountDomainModel>> createValidator,
            IValidator<ReadByIndexContext<AccountDomainModel>> readByIndexValidator,
            IValidator<UpdateContext<AccountDomainModel>> updateValidator,
            IValidator<DeleteContext<AccountDomainModel>> deleteValidator,
            IAccountStore store,
            IEasyCachingProvider cache,
            IDomainEventBus events,
            ILogger<IAccountService> logger)
        {
            _createValidator = createValidator;
            _readByIndexValidator = readByIndexValidator;
            _updateValidator = updateValidator;
            _deleteValidator = deleteValidator;
            _store = store;
            _cache = cache;
            _events = events;
            _logger = logger;
        }

        public async Task GetAccountByName(ReadByIndexContext<AccountDomainModel> context)
        {
            if (!await _readByIndexValidator.IsValidFor(context) || context.IsError)
            {
                _logger.LogInformation(context.Error);
                return;
            }

            var key = AccountCaching.BuildGetByNameKey(context.Index);
            var read = await _cache.GetAsync(key,
                   () => _store.GetAccountByName(context.Index),
                   AccountCaching.Expiration);

            context.Read = read.Value;

            if (context.Read == default)
                context.SetErrors($"Failed to find account named \'{context.Index}\'", "account was not found");

            if (context.IsError)
                _logger.LogError(context.Error);
        }

        public async Task GetAccountBySubjectId(ReadByIndexContext<AccountDomainModel> context)
        {
            if (!await _readByIndexValidator.IsValidFor(context) || context.IsError)
            {
                _logger.LogInformation(context.Error);
                return;
            }

            var key = AccountCaching.BuildGetBySubjectIdKey(context.Index);
            var read = await _cache.GetAsync(key,
                   () => _store.GetAccountBySubjectId(context.Index),
                   AccountCaching.Expiration);

            context.Read = read.Value;

            if (context.Read == default)
                context.SetErrors($"Failed to find account named \'{context.Index}\'", "account was not found");

            if (context.IsError)
                _logger.LogError(context.Error);

        }

        public async Task CreateAccount(CreateContext<AccountDomainModel> context)
        {
            if (!await _createValidator.IsValidFor(context) || context.IsError)
            {
                _logger.LogInformation(context.Error);
                return;
            }
            context.Created = await _store.Create(context.ToCreate);
            if (context.Created == default)
            {
                context.Error = "Failed to create new record";
                _logger.LogError(context.Error);
                return;
            }

            var d = new Dictionary<string, AccountDomainModel>
            {
                { AccountCaching.BuildGetBySubjectIdKey(context.Created.CreatedByUserId), context.Created },
                { AccountCaching.BuildGetByNameKey(context.Created.Name), context.Created },
            };
            _ = _cache.SetAllAsync(d, AccountCaching.Expiration);

            _ = _events.Publish(EventKeys.Account.Created, context);
        }

        public async Task UpdateAccount(UpdateContext<AccountDomainModel> context)
        {
            if (context.Before == default)
                context.Before = await GetAccountById(context.ToUpdate.Id);

            if (!await _updateValidator.IsValidFor(context) || context.IsError)
            {
                _logger.LogInformation(context.Error);
                return;
            }

            context.After = await _store.UpdateAccount(context.ToUpdate);

            if (context.After == default)
            {
                context.Error = "Failed to create new record";
                _logger.LogError(context.Error);
                return;
            }

            _ = _cache.RemoveAsync(AccountCaching.BuildGetBySubjectIdKey(context.Before.CreatedByUserId));
            _ = _cache.RemoveAsync(AccountCaching.BuildGetByNameKey(context.Before.Name));

            _ = _events.Publish(EventKeys.Account.Updated, context);
        }

        public async Task DeleteAccountById(DeleteContext<AccountDomainModel> context)
        {
            if (context.ToDelete == default)
                context.ToDelete = await GetAccountById(context.IdToDelete);

            var token = await _cache.GetAsync<string>(AccountCaching.BuildDeletionToken(context.UserId, context.IdToDelete));

            if (!token.HasValue)
            {
                context.SetErrors(
                    $"Invalid deletion token: {context.DeleteToken}",
                    "Failed to delete entry");

                _logger.LogError(context.Error);
                return;
            }
            if (!await _deleteValidator.IsValidFor(context) || context.IsError)
            {
                _logger.LogError(context.Error);
                return;
            }

            context.Deleted = await _store.DeleteAccount(context.IdToDelete);

            if (context.Deleted == default)
            {
                context.SetErrors(
                    $"Failed to delete record with \'id\' = {context.IdToDelete}",
                    $"Failed to delete record ");
                _logger.LogError(context.Error);
                return;
            }

            _ = _cache.RemoveAsync(AccountCaching.BuildGetBySubjectIdKey(context.ToDelete.CreatedByUserId));
            _ = _cache.RemoveAsync(AccountCaching.BuildGetByNameKey(context.ToDelete.Name));

            _ = _events.Publish(EventKeys.Account.Deleted, context);
        }

        private async Task<AccountDomainModel?> GetAccountById(string id)
        {
            var all = await _cache.GetByPrefixAsync<AccountDomainModel?>(AccountCaching.Prefix);

            var res = all.Values.FirstOrDefault(x => x.Value?.Id == id);
            return res.HasValue && res.Value != null ?
                await _store.GetAccountById(id) :
                res.Value;
        }

        public async Task<string> GetAccountDeletionToken(string subjectId, string accountId)
        {
            var token = Guid.NewGuid().ToString("N");

            await _cache.SetAsync(
                    AccountCaching.BuildDeletionToken(subjectId, accountId),
                    token,
                    AccountCaching.DeleteTokenExpiration);

            return token;
        }
    }
}
