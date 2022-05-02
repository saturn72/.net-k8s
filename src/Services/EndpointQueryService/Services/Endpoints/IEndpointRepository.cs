using EndpointQueryService.Domain;

namespace EndpointQueryService.Services.Endpoints
{
    public interface IEndpointRepository
    {
        Task<EndpointInfo> GetEndpointByPath(string path);
        Task<IEnumerable<EndpointEntry>> GetEntries(GetEntriesContext context);
    }
}
