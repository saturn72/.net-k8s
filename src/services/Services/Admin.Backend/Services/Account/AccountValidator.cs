using Admin.Backend.Domain;
using EasyCaching.Core;

namespace Admin.Backend.Services.Account
{
    public class AccountValidator :
        IValidator<CreateContext<AccountDomainModel>>,
        IValidator<ReadByIdContext<AccountDomainModel>>,
        IValidator<ReadByIndexContext<AccountDomainModel>>,
        IValidator<UpdateContext<AccountDomainModel>>,
        IValidator<DeleteContext<AccountDomainModel>>
    {
        private readonly IAccountStore _store;
        private readonly IEasyCachingProvider _cache;

        public AccountValidator(
            IAccountStore store,
            IEasyCachingProvider cache)
        {
            _store = store;
            _cache = cache;
        }

        public Task<bool> IsValidFor(ReadByIdContext<AccountDomainModel> context)
        {
            var res = context.Id == context.UserId;

            if (!res)
            {
                context?.SetErrors(
                    $"User is not permitted to get account. Requested by UserId: {context.UserId}, Requested account's subjectId: {context.Id}",
                    "User not authorized to read account info");
            }

            return Task.FromResult(res);
        }

        public async Task<bool> IsValidFor(CreateContext<AccountDomainModel> context)
        {
            var a = await GetAccountByName(context?.ToCreate?.Name);

            if (a?.HasValue == true)
            {
                context?.SetErrors($"Account already exist: {context.ToCreate}",
                    "Account already exist");
                return false;
            }

            return true;
        }

        public async Task<bool> IsValidFor(UpdateContext<AccountDomainModel> context)
        {
            var a = await GetAccountByName(context?.Before?.Name);

            if (a?.HasValue == true && a.Value.Id != context.Before.Id)
            {
                context?.SetErrors($"Account already exist: {context.Before}",
                    "Account already exist");
                return false;
            }

            return true;
        }

        public Task<bool> IsValidFor(DeleteContext<AccountDomainModel> context)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsValidFor(ReadByIndexContext<AccountDomainModel> context)
        {
            throw new NotImplementedException();
        }

        private async Task<CacheValue<AccountDomainModel>> GetAccountByName(string? name)
        {
            var key = AccountCaching.BuildGetByNameKey(name);
            return await _cache.GetAsync(key,
                   () => _store.GetAccountByName(name),
                   AccountCaching.Expiration);
        }
    }
}
