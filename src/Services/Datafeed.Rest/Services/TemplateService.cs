using Datafeed.Rest.Data;
using Datafeed.Rest.Domain;
using EasyCaching.Core;

namespace Datafeed.Rest.Services
{
    public class TemplateService : ITemplateService
    {
        private readonly IEasyCachingProvider _cache;
        private readonly IRepository _templateRepository;
        private readonly ILogger<TemplateService> _logger;

        public TemplateService(
            IEasyCachingProvider cache,
            IRepository templateRepository,
            ILogger<TemplateService> logger)
        {
            _cache = cache;
            _templateRepository = templateRepository;
            _logger = logger;
        }

        public async Task GetEntries(GetAllEntriesContext context)
        {
            _logger.LogDebug(nameof(GetEntries));

            var key = CacheSettings.Template.BuildGetEntriesPageKey(context);
            var entries = await _cache.GetAsync(
                key,
                () => _templateRepository.GetEntriesByVersion(context),
                CacheSettings.Template.Expiration);

            if (!entries.HasValue)
            {
                context.Error = "failed to fetch entries";
                _logger.LogError(context.Error);
                return;
            }

            context.Data = entries.Value;
        }

        public async Task<Template> GetTemplateByPath(string path)
        {
            _logger.LogDebug(nameof(GetTemplateByPath) + ": " + path);

            var key = CacheSettings.Template.BuildGetPathKey(path);
            var cv = await _cache.GetAsync(key,
                () => _templateRepository.GetTemplateByPath(path),
                CacheSettings.Template.Expiration);

            return cv?.Value;
        }
    }
}