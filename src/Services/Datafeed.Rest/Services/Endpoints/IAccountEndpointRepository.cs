using Datafeed.Rest.Domain;

namespace Datafeed.Rest.Services.Endpoints
{
    public interface IAccountEndpointRepository
    {
        Task<AccountEndpointVersion> GetLatestEntries(GetEntriesContext context);
        Task<AccountEndpointVersion> GetEntriesByVersion(GetEntriesContext context);
    }
}
