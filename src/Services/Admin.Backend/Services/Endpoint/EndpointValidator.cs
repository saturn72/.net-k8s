using Admin.Backend.Domain;
using EasyCaching.Core;

namespace Admin.Backend.Services.Endpoint
{
    public class EndpointValidator : IValidator<CreateContext<EndpointDomainModel>>
    {
        private readonly IEndpointStore _endpoints;
        private readonly IEasyCachingProvider _cache;

        public EndpointValidator(
            IEndpointStore endpoints,
            IEasyCachingProvider cache)
        {
            _endpoints = endpoints;
            _cache = cache;
        }
        public async Task<bool> IsValidFor(CreateContext<EndpointDomainModel> context)
        {
            var key = EndpointCaching.BuildGetPathKey(context?.ToCreate?.Path);
            var ep = await _cache.GetAsync(key,
                () => _endpoints.GetByPath(context.ToCreate.Path),
                EndpointCaching.Expiration);

            if (ep?.HasValue == true)
            {
                context?.SetErrors($"Endpoint already exist: {context.ToCreate}",
                    "Endpoint already exist");
                return false;
            }

            return true;
        }
    }
}
