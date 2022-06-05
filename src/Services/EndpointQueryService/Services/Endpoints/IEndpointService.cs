using EndpointQueryService.Domain;

namespace EndpointQueryService.Services.Endpoints
{
    public interface IEndpointService
    {
        Task GetEndpointPage(GetEntriesContext context);
        Task<EndpointInfo> GetEndpointInfoByPath(string path);
    }
}
