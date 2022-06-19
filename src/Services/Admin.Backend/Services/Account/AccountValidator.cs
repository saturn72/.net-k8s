using Admin.Backend.Domain;
using EasyCaching.Core;

namespace Admin.Backend.Services.Account
{
    public class AccountValidator : IValidator<CreateContext<AccountDomainModel>>
    {
        private readonly IAccountStore store;
        private readonly IEasyCachingProvider _cache;

        public AccountValidator(
            IAccountStore store,
            IEasyCachingProvider cache)
        {
            this.store = store;
            _cache = cache;
        }
        public async Task<bool> IsValidFor(CreateContext<AccountDomainModel> context)
        {
            var key = AccountCaching.BuildGetNameKey(context?.Model?.Name);
            var ep = await _cache.GetAsync(key,
                () => store.GetByName(context.Model.Name),
                AccountCaching.Expiration);

            if (ep?.HasValue == true)
            {
                context?.SetErrors($"Account already exist: {context.Model}",
                    "Account already exist");
                return false;
            }

            return true;
        }
    }
}
