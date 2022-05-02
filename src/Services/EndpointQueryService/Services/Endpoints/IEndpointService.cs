using EndpointQueryService.Domain;

namespace EndpointQueryService.Services.Endpoints
{
    public interface IEndpointService
    {
        Task GetEntriesPage(GetEntriesContext context);
        Task<EndpointInfo> GetEndpointInfoByPath(string path);
    }
}
