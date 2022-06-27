using Admin.Backend.Domain;
using EasyCaching.Core;

namespace Admin.Backend.Services.Datasource
{
    public class DatasourceValidator : IValidator<CreateContext<DatasourceDomainModel>>
    {
        private readonly IDatasourceStore store;
        private readonly IEasyCachingProvider _cache;

        public DatasourceValidator(
            IDatasourceStore store,
            IEasyCachingProvider cache)
        {
            this.store = store;
            _cache = cache;
        }
        public async Task<bool> IsValidFor(CreateContext<DatasourceDomainModel> context)
        {
            var key = DatasourceCaching.BuildGetNameKey(context?.ToCreate?.Name);
            var ep = await _cache.GetAsync(key,
                () => store.GetByName(context.ToCreate.Name),
                DatasourceCaching.Expiration);

            if (ep?.HasValue == true)
            {
                context?.SetErrors($"Datasource already exist: {context.ToCreate}",
                    "Datasource already exist");
                return false;
            }

            return true;
        }
    }
}
