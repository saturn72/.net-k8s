using EasyCaching.Core;

namespace Datafeed.Rest.Services.Rate
{
    public interface IRateRecordRepository
    {

    }
    public class RateManager : IRateManager
    {
        private readonly IEasyCachingProvider _cache;
        private readonly IRateRecordRepository _reporitory;
        private readonly ILogger<RateManager> _logger;

        public RateManager(
            IEasyCachingProvider cache,
            IRateRecordRepository rateRepository,
             ILogger<RateManager> logger)
        {
            _cache = cache;
            _reporitory = rateRepository;
            _logger = logger;
        }
        public Task IncrementAccessToAccountEndpointVersionAction(ActionContext context)
        {
            throw new NotImplementedException();
        }
        public Task<bool> UserExceededAccessToAccountEndpointVersionAction(ActionContext context)
        {
            //get user rates
            //check if excceeded
            throw new NotImplementedException();
        }
    }
}
