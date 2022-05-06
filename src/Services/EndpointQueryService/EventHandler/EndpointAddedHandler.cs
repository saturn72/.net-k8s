using EndpointQueryService.Data.Endpoints;
using EndpointQueryService.Domain;
using EndpointQueryService.Services;

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
            var isLatest = bool.Parse(j["latest"]);

            var repo = services.GetRequiredService<IEndpointRepository>();
            var entry = new EndpointInfo
            {
                Account = account,
                Name = name,
                Version = version,
            };

            await repo.AddOrUpdateEndpointEntryByPath(entry);
            if (isLatest)
            {
                var l = entry with { Version = Consts.Endpoint.LatestVersion };
                await repo.AddOrUpdateEndpointEntryByPath(entry);
            }

            //set latest

            //var model = new EndpointDbModel
            //path
            //search-tags
            //content
        };
    }
}
