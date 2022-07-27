using EndpointQueryService.Domain;
using EndpointQueryService.Services;

namespace EndpointQueryService.Data.Endpoints
{
    public interface IEndpointRepository
    {
        Task<EndpointInfo> GetEndpointByPath(string path);
        Task<IEnumerable<EndpointEntry>> GetEntriesPage(GetEntriesContext context);
        Task AddOrUpdateEndpointEntryByPath(EndpointInfo info);
    }
}
