using EasyCaching.Core;

namespace EndpointQueryService.Services.Rate
{
    public class RateManager : IRateManager
    {
        private readonly ILogger<RateManager> _logger;

        public RateManager(
             ILogger<RateManager> logger)
        {
            _logger = logger;
        }
        public Task IncrementAccessToAccountEndpointVersionAction(ActionContext context)
        {
            return Task.CompletedTask;
        }
        public Task<bool> UserExceededAccessToAccountEndpointVersionAction(ActionContext context)
        {
            return Task.FromResult(false);
        }
    }
}
