using EndpointQueryService.Domain;
using EndpointQueryService.Services;

namespace EndpointQueryService.Data.Endpoints
{
    public interface IEndpointRepository
    {
        Task<EndpointInfo> GetEndpointByPathAsync(string path);
        Task<IEnumerable<EndpointEntry>> GetEntries(GetEntriesContext context);
        Task AddOrUpdateEndpointEntryByPath(EndpointInfo info);
    }
}
