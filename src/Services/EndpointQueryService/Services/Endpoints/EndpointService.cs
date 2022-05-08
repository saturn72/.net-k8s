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

        public async Task GetEntriesPage(GetEntriesContext context)
        {
            _logger.LogDebug(nameof(GetEntriesPage));

            var key = CacheSettings.Endpoint.BuildGetEntriesPageKey(context);
            var entries = await _cache.GetAsync(
                key,
                () => _endpointRepository.GetEntries(context),
                CacheSettings.Endpoint.Expiration);

            if (entries == default || entries.IsNull || !entries.HasValue)
            {
                context.Error = "failed to fetch entries";
                _logger.LogError(context.Error);
                return;
            }

            context.Data = entries.Value.Select(d => d.Data).ToList();
        }

        public async Task<EndpointInfo> GetEndpointInfoByPath(string path)
        {
            _logger.LogDebug(nameof(GetEndpointInfoByPath) + ": " + path);

            var key = CacheSettings.Endpoint.BuildGetPathKey(path);
            var cv = await _cache.GetAsync(key,
                () => _endpointRepository.GetEndpointByPathAsync(path),
                CacheSettings.Endpoint.Expiration);

            return cv?.Value;
        }
    }
}