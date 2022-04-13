using Datafeed.Rest.Domain;

namespace Datafeed.Rest.Services.Endpoints
{
    public interface IEndpointService
    {
        Task<AccountEndpoint> GetEndpointByName(Account account, string endpointName);
        Task GetEntries(GetEntriesContext context);
    }
}
