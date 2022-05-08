using EndpointQueryService.Data.Endpoints;
using EndpointQueryService.Domain;

namespace EndpointQueryService.EventHandler
{
    public class EndpointAddedHandler
    {
        public Func<IServiceProvider, object, Task> Handle => async (services, @event) =>
        {
            var j = @event as IDictionary<string, string>;
            string account = j["account"],
                name = j["name"],
                version = j["version"];

            var repo = services.GetRequiredService<IEndpointRepository>();
            var entry = new EndpointInfo
            {
                Account = account,
                Name = name,
                Version = version,
            };

            await repo.AddOrUpdateEndpointEntryByPath(entry);

            throw new NotImplementedException("handle deletion and endpoint expiration");
        };
    }
}
