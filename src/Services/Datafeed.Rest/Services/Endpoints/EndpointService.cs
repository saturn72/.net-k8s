using Datafeed.Rest.Domain;
using EasyCaching.Core;

namespace Datafeed.Rest.Services.Endpoints
{
    public class EndpointService : IEndpointService
    {
        private readonly IEasyCachingProvider _cache;
        private readonly IAccountEndpointRepository _repository;
        private readonly ILogger<EndpointService> _logger;

        public EndpointService(
            IEasyCachingProvider cache,
            IAccountEndpointRepository repository,
            ILogger<EndpointService> logger)
        {
            _cache = cache;
            _repository = repository;
            _logger = logger;
        }

        public Task<AccountEndpoint> GetEndpointByName(Account account, string endpointName)
        {
            throw new NotImplementedException();
        }

        public async Task GetEntries(GetEntriesContext context)
        {
            _logger.LogDebug(nameof(GetEntries));

            var key = AccountEndpointVersionCacheSettings.GetEntries.GetEntriesPageKey(context);
            var entries = await _cache.GetAsync(
                key,
                () => getAccountEndpointVersionEntries(),
                AccountEndpointVersionCacheSettings.GetEntries.Expiration);

            if (!entries.HasValue)
            {
                context.Error = "failed to fetch entries";
                _logger.LogError(context.Error);
                return;
            }

            context.Data = entries.Value;

            Task<AccountEndpointVersion> getAccountEndpointVersionEntries()
            {
                return context.Version.Equals(Consts.AccountEndpointVersion.LatestAlias, StringComparison.InvariantCultureIgnoreCase) ?
                _repository.GetLatestEntries(context) :
                _repository.GetEntriesByVersion(context);
            }
        }
    }
}