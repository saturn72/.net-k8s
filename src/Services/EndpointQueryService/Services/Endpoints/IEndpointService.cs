using EndpointQueryService.Domain;

namespace EndpointQueryService.Services.Endpoints
{
    public interface IEndpointService
    {
        Task GetEntries(GetEntriesContext context);
        Task<EndpointInfo> GetEndpointInfoByPath(string path);
    }
}
