using EndpointQueryService.Data.Endpoints;
using EndpointQueryService.Domain;
using Microsoft.AspNetCore.Authentication;

namespace EndpointQueryService.EventHandler
{
    public class EndpointAddedHandler
    {
        public Func<IServiceProvider, object, Task> Handle => async (services, @event) =>
        {
            var j = @event as IDictionary<string, string>;

            var repo = services.GetRequiredService<IEndpointRepository>();
            var clock = services.GetService<ISystemClock>();
            var entry = new EndpointInfo
            {
                Meta = new EndpointMeta
                {
                    PublishedOnUtc = clock.UtcNow.UtcDateTime,
                    Account = j["account"],
                    Name = j["name"],
                    Version = j["version"],
                    SubVersion = j["subversion"]
                }
            };

            await repo.AddOrUpdateEndpointEntryByPath(entry);

            throw new NotImplementedException("handle deletion and endpoint expiration");
        };
    }
}
