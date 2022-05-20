using EasyCaching.Core;
using EndpointQueryService.Data.Endpoints;
using EndpointQueryService.Domain;

namespace EndpointQueryService.Services.Endpoints
{
    public class EndpointService : IEndpointService
    {
        private readonly IEasyCachingProvider _cache;
        private readonly IEndpointRepository _endpointRepository;
        private readonly ILogger<EndpointService> _logger;

        public EndpointService(
            IEasyCachingProvider cache,
            IEndpointRepository endpointRepository,
            ILogger<EndpointService> logger)
        {
            _cache = cache;
            _endpointRepository = endpointRepository;
            _logger = logger;
        }

        public async Task GetEntries(GetEntriesContext context)
        {
            _logger.LogDebug(nameof(GetEntries));
            if (context.Ids?.Any() == true)
            {
                await GetEntriesByIds(context);
            }

            else if (context.Filter?.Any() == true)
            {
                await GetFilteredEntries(context);
            }
            else
            {
                await GetEntriesPage(context);
            }
        }
        protected async Task GetFilteredEntries(GetEntriesContext context)
        {

            throw new NotImplementedException();
        }
        protected async Task GetEntriesByIds(GetEntriesContext context)
        {
            var allIdEntries = await _cache.GetByPrefixAsync<object>(CacheSettings.Endpoint.ByIdPrefix(context));

            var toFetch = allIdEntries.Where(kvp => !context.Ids.Contains(kvp.Key));

            //in case all entries already in cache
            if(!toFetch.Any())
            {
                context.Data = allIdEntries.Select(x => x.Value.Value);
                return;
            }
            _endpointRepository.GetEntriesPage(context);
            for (int i = 0; i < allIdEntries.Count; i++)
            {

            }
            if(allIdEntries == null)
            var allKeys = context.Ids.Select(id => CacheSettings.Endpoint.BuildGetEntryByIdKey(context, id)).ToArray();
            var toFetch = new List<string>();
            var data = new List<object>();

            for (int i = 0; i < allKeys.Count(); i++)
            {

            var  = await _cache.GetAsync<object>(CacheSettings.Endpoint.Prefix);

            }
            var listRes = new List<object>();
            throw new NotImplementedException();
        }
        protected async Task GetEntriesPage(GetEntriesContext context)
        {
            var key = CacheSettings.Endpoint.BuildGetEntriesPageKey(context);
            var entries = await _cache.GetAsync(
                key,
                async () =>
                {
                    var entries = await _endpointRepository.GetEntriesPage(context);
                    return entries.Select(x => x.Data).ToList().AsEnumerable();
                },
                CacheSettings.Endpoint.Expiration);

            if (entries == default || entries.IsNull || !entries.HasValue)
            {
                context.Error = "failed to fetch entries";
                _logger.LogError(context.Error);
                return;
            }

            context.Data = entries.Value;
        }

        public async Task<EndpointInfo> GetEndpointInfoByPath(string path)
        {
            _logger.LogDebug(nameof(GetEndpointInfoByPath) + ": " + path);

            var key = CacheSettings.Endpoint.BuildGetPathKey(path);
            var cv = await _cache.GetAsync(key,
                () => _endpointRepository.GetEndpointByPath(path),
                CacheSettings.Endpoint.Expiration);

            return cv?.Value;
        }
    }
}